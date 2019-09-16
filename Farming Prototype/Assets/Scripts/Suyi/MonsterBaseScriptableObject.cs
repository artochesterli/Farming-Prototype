using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterBaseScriptableObject : ScriptableObject
{
	public float MovingSpeed = 1f;
	public float IdleMinDuration = 0.5f;
	public float IdleMaxDuration = 4f;

	public float IdleMoveMinDuration = 0.5f;
	public float IdleMoveMaxDuration = 1.5f;

	public float FleeMinDuration = 1.5f;
	public float FleeMaxDuration = 2.5f;
	public float FleeMovingSpeed = 1.5f;

	/// <summary>
	/// All Sights Combined
	/// </summary>
	public List<Sight> Sights;
	public GameObject ExclamationPrefab;
	public float ExclamationDuration;
}
