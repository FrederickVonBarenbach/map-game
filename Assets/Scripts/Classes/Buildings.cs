 using System.Collections.Generic;
using System.Linq;
using System;

//TODO:
//- add abundance modifier and other effect modifiers

public class Building
{
    public string name; //name of building
    public BuildingType type; //kind of building
    public Organization owner; //owner of building
    public Settlement settlement; //where it exists

    public Good[] buildEffects;

    public Building(string name, string type, Organization owner, Settlement settlement)
    {
        this.name = name;
        this.type = Types.buildings[type];
        this.owner = owner;
        this.settlement = settlement;

        BuildEffects();
    }

    public void AddEffects()
    {
        foreach(Good effect in buildEffects)
        {
            Good.Add(owner.goods, effect);
        }
    }

    public void RemoveEffects()
    {
        foreach (Good effect in buildEffects)
        {
            effect.stock = 0;
            Good.Remove(owner.goods, effect);
        }
    }

    public float AbundanceBonus(string type)
    {
        return settlement.region.availableGoods.
            Find(x => x.type == Types.goods[type]).abundance;
    }

    public void BuildEffects()
    {
        buildEffects = type.buildEffects(this);
        AddEffects();
    }
}

//==============================================================================

//There should be an option for manufacturies? which makes it so that you can
//import goods from other towns while non-manufacturies gather resources and
//can only be built in zones where the resource exists

public class BuildingType
{
    public string name
    {
        get { return Types.buildings.FirstOrDefault(x => x.Value == this).Key; }
    }

    public Func<Building, Good[]> buildEffects;

    public BuildingType(Func<Building, Good[]> buildEffects)
    {
        this.buildEffects = buildEffects;
    }
}

/*public class BuildEffect
{
    public string type;
    public string cost;
    public string rateMod;
    public string rateMlt;

    public BuildEffect(string type, string cost, string rateMod = "0", string rateMlt = "1")
    {
        this.type = type;
        this.cost = cost;
        this.rateMod = rateMod;
        this.rateMlt = rateMlt;
    }
}*/