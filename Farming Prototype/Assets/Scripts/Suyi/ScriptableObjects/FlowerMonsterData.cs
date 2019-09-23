using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FlowerData", menuName = "FarmingPrototype/Monster/FlowerData", order = 1)]
public class FlowerMonsterData : MonsterBaseScriptableObject3D
{
    public float ShootAngle;
    public float ShootRadius;
    public float TurnTendency = 1f;
    public float PushForce = 20f;
    public float PushDuration = 0.5f;
    public float ResetTime = 2f;
    public GameObject PushFieldPrefab;
}
