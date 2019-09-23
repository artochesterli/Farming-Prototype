using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuffaloData", menuName = "FarmingPrototype/Monster/BuffaloData", order = 1)]
public class BuffaloData : MonsterBaseScriptableObject3D
{
    public float StareMinDuration = 0.5f;
    public float StareMaxDuration = 1f;
    public float ChargeSpeed = 10f;
    public float TurnTendency = 1f;
    public float OverChargeTime = 1f;
    public float PushForce = 500f;
    public float PushDuration = 0.5f;
}
