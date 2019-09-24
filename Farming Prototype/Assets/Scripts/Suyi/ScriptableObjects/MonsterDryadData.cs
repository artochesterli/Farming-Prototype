using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DryadData", menuName = "FarmingPrototype/Monster/DryadData", order = 1)]
public class MonsterDryadData : MonsterBaseScriptableObject3D
{
    public float NormalSpeed = 12;
    public float StickySlowDownSpeed = 1;
    public float PushSpeed = 12;

    public float DodgeDis = 6;
    public float DodgeTime = 0.2f;
    public float DodgeCoolDown = 2;

    public int ThornNumber = 3;
}
