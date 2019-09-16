using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SlimeData", menuName = "FarmingPrototype/Monster/SlimeData", order = 1)]
public class SlimeData : MonsterBaseScriptableObject
{
	public GameObject SlimeWaterPrefab;
	public float SlimeWaterExistanceDuration = 10f;
	public float SlimeWaterEmitInterval = 1f;
	public float SlimeWaterSuprisedEmitInterval = 0.5f;
}
