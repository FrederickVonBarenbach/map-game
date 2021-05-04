using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//ORGANIZATIONS WILL SELL DISCRETE GOODS IN PACKETS OF A "GOOD CLASS" WHERE WE HAVE THE
//RESOURCE TYPE, AMOUNT, PRICE, ORGANIZATION TO WHICH IT BELONGS, THOSE WHO PURCHASE IT
//WILL GIVE MONEY DIRECTLY TO THE ORGANIZATION THROUGHT THE MARKETPLACE

//ORGANIZATIONS SHOULD HAVE A LIST OF OWNED BUILDINGS AND OTHER OWNED STUFF


//WE HAVE A LIST OF ALL ORGANIZATIONS IN PLAY
//LETS CALL IT ORGLIST

//WE HAVE A FUNCTION THAT IS A COROUTINE THAT GOES THROUGH THE LIST


/*COROUTINE {
 * for num sections in orglist (length / n) {
 *      for each org in section of orglist {

 *          call the organization AI function for that org
 *      }
 *      yield return null or whatever
 * }
//NOTE: we basically seperate list into sections which
//      are the num of orgs we look at per frame
//then we call this coroutine again just like clock (this is infinite loop)
//that staggers the calling of the org ai
}*/

[System.Serializable]
public class Organization
{
    [HideInInspector]
    #if UNITY_EDITOR
    public string name;
    #endif
    public OrganizationType type;

    public List<Good> goods;

    public void IncreaseStockpile()
    {
        foreach (Good good in goods)
        {
            good.stock += good.rate;
        }
    }


    public Organization(string name, string type)
    {
        this.name = name;
        this.type = Types.organizations[type];
    }

}

//==============================================================================

public class OrganizationType
{
    public string name
    {
        get { return Types.organizations.FirstOrDefault(x => x.Value == this).Key; }
    }

    public OrganizationType()
    {

    }
}