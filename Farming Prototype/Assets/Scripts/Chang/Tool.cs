using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tool
{
    public string name;
}


public class Shovel : Tool
{
    public Shovel()
    {
        name = "Shovel";
    }
}

public class WaterKettle : Tool
{
    public WaterKettle()
    {
        name = "Water Kettle";
    }
}


public class CaptureNet : Tool
{
    public CaptureNet()
    {
        name = "Capture Net";
    }

}
