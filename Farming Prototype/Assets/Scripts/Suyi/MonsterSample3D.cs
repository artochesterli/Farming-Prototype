using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSample3D : MonsterBase3D
{
    public override bool OnHit(float chance)
    {
        float rand = Random.Range(0f, 1f);
        // If capture success
        if (rand < chance)
        {
            OnCapture();
            return true;
        }
        else
        {
            Capturable = true;
        }
        return false;
    }

    public override void OnCapture()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }

    public override void OnUse(PlayerController pc)
    {
        throw new System.NotImplementedException();
    }
}
