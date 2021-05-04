using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEditor;

public partial class RegionLoader : MonoBehaviour
{
    public GameObject regionOverlay;
    public List<RegionData> regions;

    protected Texture2D myTexture;
    protected Sprite mySprite;
    protected float scale;

    // Start is called before the first frame update
    void Start()
    {
        mySprite = GetComponent<SpriteRenderer>().sprite;
        myTexture = mySprite.texture;
        scale = myTexture.width / mySprite.bounds.size.x;

        SortRegions();
        //CreateRegions uses the maths of colors in it as well so we need
        //to sort before we make the regions or we will get stuck!
        CreateRegions();
    }

    // Update is called once per frame
    void Update()
    {
        InputHandler();
    }

    //==============================================================================

    void SortRegions()
    {
        //Only works for red at the moment
        regions.Sort(delegate (RegionData r1, RegionData r2)
        {
            float c1 = r1.color.r;
            float c2 = r2.color.r;
            return c2.CompareTo(c1);
        });
    }

    void CreateRegions()
    {
        List<Tuple<Region, RegionData>> regionList = new List<Tuple<Region, RegionData>>();

        //Check how long this all took
        DateTime startTime = DateTime.Now;

        foreach (RegionData region in regions)
        {
            //Place in correct position
            GameObject regionGO = new GameObject(region.name);
            regionGO.transform.position = region.centre;
            regionGO.transform.SetParent(regionOverlay.GetComponent<Transform>());

            //Make texture (currently 400x400)
            Texture2D texture = new Texture2D(400, 400, TextureFormat.RGBA32, false);
            texture.filterMode = FilterMode.Point;

            //Add color from Region Map to texture
            Vector2 startPos = new Vector2(scale * (region.centre.x + mySprite.bounds.size.x / 2) - 200,
                                           scale * (region.centre.y + mySprite.bounds.size.y / 2) - 200);
            Color[] cols = myTexture.GetPixels((int)startPos.x, (int)startPos.y, 400, 400);
            for (int i = 0; i < cols.Length; i++)
            {
                if (cols[i].a != 0 &&
                    (int)Mathf.Round(cols[i].r * 51) == (int)Mathf.Round(region.color.r * 51))
                {
                    cols[i] = Color.white;
                }
                else
                {
                    cols[i] = new Color(0, 0, 0, 0);
                }
            }
            texture.SetPixels(cols);

            //Make sprite and add color
            SpriteRenderer sr = regionGO.AddComponent<SpriteRenderer>();
            sr.sprite = Sprite.Create(texture,
                                      new Rect(0, 0, texture.width, texture.height),
                                      new Vector2(0.5f, 0.5f),
                                      scale);
            sr.sortingOrder = 1;
            sr.color = Color.yellow;
            texture.Apply(false);

            //Add components to region
            regionGO.AddComponent<PolygonCollider2D>();
            regionGO.tag = "Region";
            regionGO.AddComponent<Region>();

            //Add goods
            foreach(RegionData.GoodData good in region.goods)
            {
                regionGO.GetComponent<Region>().availableGoods
                    .Add(new Region.RegionGood(good.type, good.abundance));
            }

            //Add this region to the regionList (for adjRegion instantiation)
            regionList.Add(new Tuple<Region, RegionData>(regionGO.GetComponent<Region>(), region));
        }

        //ONLY WORKS FOR RED AT THE MOMENT
        foreach (Tuple<Region, RegionData> Region in regionList) {
            foreach (Color adjCol in Region.Item2.adjRegions)
            {
                int index = 51 - (int)Mathf.Round(adjCol.r * 255 / 5);
                //Multiplying by 60km since that is the scale factor
                float dist = Tools.Dcm(60*Vector3.Distance(regions[index].centre,
                                                        Region.Item1.transform.position), 0);
                Region.Item1.GetComponent<Region>().adjRegions
                    .Add(new Region.AdjRegion(regionList[index].Item1, dist));
            }
        }

        Debug.Log("===============================");
        Debug.Log("It took " + DateTime.Now.Subtract(startTime).Milliseconds + "ms to load all regions");
        Debug.Log("===============================");
    }

    //==============================================================================

    protected Color GetColor()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 pos = new Vector2(mousePos.x + mySprite.bounds.size.x / 2,
                                  mousePos.y + mySprite.bounds.size.y / 2);
        return myTexture.GetPixel((int)(pos.x * scale),
                                  (int)(pos.y * scale));
    }

    protected Vector2 GetMousePos()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return new Vector2(Tools.Dcm(mousePos.x), Tools.Dcm(mousePos.y));
    }

    [System.Serializable]
    public class RegionData
    {
        #if UNITY_EDITOR
        public string name;
        #endif
        public Vector2 centre;
        public Color color;
        [HideInInspector]
        public List<Color> adjRegions;
        
        public List<GoodData> goods;
        public List<SettlementData> settlements;

        [CustomPropertyDrawer(typeof(GoodData))]
        public class GoodDataDrawer : PropertyDrawer
        {
            int _choiceIndex;
            string[] _choices = Types.goods.Values.Select(x => x.name).ToArray();

            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                var selectionRect = new Rect(position.x, position.y, 150, position.height);
                var abundanceRect = new Rect(position.x + 150, position.y, 100, position.height);

                EditorGUI.BeginChangeCheck();
                int index = Array.IndexOf(_choices, property.FindPropertyRelative("name").stringValue);
                _choiceIndex = EditorGUI.Popup(selectionRect, index, _choices);
                if (EditorGUI.EndChangeCheck()) property.FindPropertyRelative("name").stringValue = _choices[_choiceIndex];

                EditorGUI.PropertyField(abundanceRect, property.FindPropertyRelative("abundance"), GUIContent.none);
            }
        }

        [System.Serializable]
        public class GoodData
        {
            public string name;
            public GoodType type { get { return Types.goods[name]; } }
            public float abundance;

            public GoodData(string name, float abundance)
            {
                this.name = name;
                this.abundance = abundance;
            }
        }

        [System.Serializable]
        public class SettlementData
        {
            public string name;
            public SettlementType type;

            public SettlementData(string name, string type)
            {
                this.name = name;
                this.type = Types.settlements[type];
            }
        }

        public RegionData(string name, Vector2 centre, Color color)
        {
            this.name = name;
            this.centre = centre;
            this.color = color;
        }
    }
}