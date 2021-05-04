using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class Settlement
{
    public string name;
    public SettlementType type;
    public Region region;

    public List<Building> buildings;

    public Settlement(string name, string type, Region region)
    {
        this.name = name;
        this.type = Types.settlements[type];
        this.region = region;
    }

    public void Build(string type, string name, Organization owner)
    {
        if (name == "")
        {
            name = owner.name + "'s " + this.name + " " + type;
        }
        buildings.Add(new Building(type, name, owner, this));
    }
}

//==============================================================================

public class SettlementType
{
    public string name
    {
        get { return Types.settlements.FirstOrDefault(x => x.Value == this).Key; }
    }

    public SettlementType()
    {

    }
}
