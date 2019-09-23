using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterTransform
{
    public abstract void OnUse();
}

public class BuffaloTransform : MonsterTransform
{
    public BuffaloTransform()
    {

    }
    public override void OnUse()
    {
        Debug.Log("On Use");
    }
}

public class FlowerTransform : MonsterTransform
{
    public FlowerTransform()
    {

    }
    public override void OnUse()
    {
        Debug.Log("On Use");
    }
}