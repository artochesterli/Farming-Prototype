using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SlimeData", menuName = "FarmingPrototype/Monster/SlimeData", order = 1)]
public class SlimeData : MonsterBaseScriptableObject3D
{
    public float NormalSpeed = 6;
    public float StickySlowDownSpeed = 6;
    public float PushSpeed = 6;

    public float FieldInitSize = 1;
    public float FieldMaxSize = 4;
    public float FieldGenerationTime = 2;
    public float FieldGenerationCoolDown = 3;
}
