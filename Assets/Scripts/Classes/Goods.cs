using System.Collections.Generic;
using System.Linq;

public class Good
{
    public GoodType type;
    public float stock; // in Resource

    public float rateMod; // in  Resource per month
    public float rateMlt; // in x100%
    public float rate // in  Resource per month
    {
        get { return rateMod * rateMlt; }
    }

    public Good(string type, float stock, float rateMod = 0, float rateMlt = 1)
    {
        this.type = Types.goods[type];
        this.stock = stock;
        this.rateMlt = rateMlt;
        this.rateMod = rateMod;
    }

    public void Rate()
    {
        stock += rate;
    }

    //Generic Functions

    public static void Add(List<Good> listofgoods, Good good)
    {
        Good good_ = listofgoods.Find(x => x.type == good.type);
        if (good_ != null)
        {
            good_.stock += good.stock;
            good_.rateMlt *= good.rateMlt;
            good_.rateMod += good.rateMod;
        }
        //Add the good to the list if it is not there already
        else
        {
            listofgoods.Add(good);
        }
    }

    public static void Remove(List<Good> listofgoods, Good good)
    {
        Good good_ = listofgoods.Find(x => x.type == good.type);
        good_.stock -= good.stock;
        good_.rateMlt /= good.rateMlt;
        good_.rateMod -= good.rateMod;

        //Remove if the good values have been reset (to improve performance)
        if (good_.stock == 0 && good_.rateMlt == 1 && good_.rateMod == 0)
        {
            listofgoods.Remove(good_);
        }
    }
}

//==============================================================================

public class GoodType
{
    public string name
    {
        get { return Types.goods.FirstOrDefault(x => x.Value == this).Key;  }
    }

    public float basePrice; //for equal d/s in silver

    public GoodType(float basePrice)
    {
        this.basePrice = basePrice;
    }
}
