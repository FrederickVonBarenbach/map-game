using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Region : MonoBehaviour
{
    public List<AdjRegion> adjRegions = new List<AdjRegion>();
    public List<RegionGood> availableGoods = new List<RegionGood>();
    public List<Settlement> settlements = new List<Settlement>();

    //TODO:
    //- add settlements
    //- add buildings
    //- add resources from buildings and updates to all organizations

    private Main main;

    // Start is called before the first frame update
    void Start()
    {
        main = GameObject.Find("Map").GetComponent<Main>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //==============================================================================

    [System.Serializable]
    public class AdjRegion
    {
        [HideInInspector]
        #if UNITY_EDITOR
        public string name;
        #endif
        public Region region;
        public float distance; //in km

        //if river crossing here? and maybe terrain type

        public AdjRegion(Region region, float distance)
        {
            this.region = region;
            this.distance = distance;
            name = region.name;
        }
    }

    [System.Serializable]
    public class RegionGood
    {
        [HideInInspector]
        #if UNITY_EDITOR
        public string name;
        #endif
        [HideInInspector]
        public GoodType type;
        public float abundance;

        public RegionGood(GoodType type, float abundance)
        {
            this.type = type;
            this.abundance = abundance;
            name = type.name;
        }
    }
}
