using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monster
{
    public string name;
    public int number;
}

public class WaterElement : Monster
{
    public WaterElement()
    {
        name = "Water Element";
        number = 0;
    }
}

