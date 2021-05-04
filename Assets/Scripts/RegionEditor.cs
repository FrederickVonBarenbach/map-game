using System;
using UnityEngine;

public partial class RegionLoader
{
    public GameObject arrow;

    protected RegionData selectedRegion;
    protected GameObject selected;

    void InputHandler()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            SelectRegion();
            Debug.Log("Selected Region");
            DisplaySelectedRegion(true);
        }
        if (Input.GetKeyDown("n"))
        {
            regions.Add(new RegionData("NEW REGION", GetMousePos(), GetColor()));
        }
        else if (Input.GetKeyDown("c"))
        {
            ChangeRegion(selectedRegion, (RegionData region) =>
            {
                Debug.Log("Centre Changed");
                region.centre = GetMousePos();
                DisplaySelectedRegion(false);
            });
        }
        else if (Input.GetKeyDown("x"))
        {
            ChangeRegion(selectedRegion, (RegionData region) =>
            {
                Color c = GetColor();
                if (c != selectedRegion.color)
                {
                    if (region.adjRegions.Contains(c))
                    {
                        region.adjRegions.Remove(c);
                        Debug.Log("Adjacent Region Removed");
                    }
                    else
                    {
                        region.adjRegions.Add(c);
                        Debug.Log("Adjacent Region Added");
                    }
                    DisplaySelectedRegion(false);
                }
            });
        }
    }

    //==============================================================================

    void ChangeRegion(RegionData region, Action<RegionData> operation)
    {
        Color c = region.color;
        //Only works for red at the moment
        int index = 51 - (int)Mathf.Round(c.r * 255 / 5);
        if (index >= regions.Count || c.a == 0)
        {
            throw new System.InvalidOperationException("Selected region does not yet exist");
        }
        else
        {
            if (operation != null) operation(regions[index]);
        }
    }

    void SelectRegion()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 pos = new Vector2(mousePos.x + mySprite.bounds.size.x / 2,
                                  mousePos.y + mySprite.bounds.size.y / 2);
        Color c = myTexture.GetPixel((int)(pos.x * scale),
                                     (int)(pos.y * scale));

        //Only works for red at the moment
        int index = 51 - (int)Mathf.Round(c.r * 255 / 5);
        if (index >= regions.Count || c.a == 0)
        {
            throw new System.InvalidOperationException("Selected region does not yet exist");
        }
        else
        {
            selectedRegion = (regions[index]);
        }
    }

    void DisplaySelectedRegion(bool log)
    {
        //Only works for red
        if (log)
        {
            Debug.Log("===============================");
            Debug.Log("Region: " + selectedRegion.name);
        }

        if (selected != null) Destroy(selected);
        selected = new GameObject(selectedRegion.name);
        selected.transform.position = selectedRegion.centre;

        foreach (Color col in selectedRegion.adjRegions)
        {
            RegionData adjRegion = regions[51 - (int)Mathf.Round(col.r * 255 / 5)];
            GameObject _arrow = Instantiate(arrow);
            _arrow.transform.position = selectedRegion.centre;
            float angle = Mathf.Atan2(adjRegion.centre.y - selectedRegion.centre.y,
                                      adjRegion.centre.x - selectedRegion.centre.x) * Mathf.Rad2Deg
                                      + 180;
            _arrow.transform.Rotate(0, 0, angle);
            _arrow.transform.SetParent(selected.GetComponent<Transform>());

            if (log) Debug.Log("-> " + adjRegion.name);
        }
        if (log) Debug.Log("===============================");
    }
}
