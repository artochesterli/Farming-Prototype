using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterBase3D : MonoBehaviour, ICapturable
{
    public virtual void OnCapture()
    {
    }

    public virtual void OnHit(float chance)
    {
        print(chance);
    }
}
