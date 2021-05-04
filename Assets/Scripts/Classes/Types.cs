using System.Collections.Generic;

public static class Types
{
    //All good types
    public static Dictionary<string, GoodType> goods = new Dictionary<string, GoodType>
    {
        ["Silver"] = new GoodType(1),
        ["Iron"] = new GoodType(4),
        ["Wool"] = new GoodType(2),
        ["Fish"] = new GoodType(0.5f),
        ["Grain"] = new GoodType(0.5f),
        ["Lumber"] = new GoodType(1),
        ["Stone"] = new GoodType(1.2f)
    };

    //All building types
    public static Dictionary<string, BuildingType> buildings = new Dictionary<string, BuildingType>
    {
        ["Iron Mine"] = new BuildingType(delegate(Building bldg) {
            return new Good[] {
                new Good("Silver", -400, -5),
                new Good("Iron", 0, 10 * bldg.AbundanceBonus("Iron"))
            }; }),

        ["Forestry Outpost"] = new BuildingType(delegate (Building bldg) {
            return new Good[] {
                new Good("Silver", -150, -3),
                new Good("Lumber", 0, 10 * bldg.AbundanceBonus("Lumber"))
            };
        })
    };

    //All organization types
    public static Dictionary<string, OrganizationType> organizations = new Dictionary<string, OrganizationType>
    {
        ["Governmental Body"] = new OrganizationType(),
        ["Private Company"] = new OrganizationType()
    };

    //All settlement types
    public static Dictionary<string, SettlementType> settlements = new Dictionary<string, SettlementType>
    {
        ["Village"] = new SettlementType(),
        ["Town"] = new SettlementType(),
        ["City"] = new SettlementType(),
        ["Fortress"] = new SettlementType()
    };
}
