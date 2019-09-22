using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSample3D : MonsterBase3D
{
    public override void OnHit(float chance)
    {
        base.OnHit(chance);
        float rand = Random.Range(0f, 1f);
        // If capture success
        if (rand < chance)
        {
            OnCapture();
        }
        // If capture not success
        else
        {

        }
    }

    public override void OnCapture()
    {

    }
}
