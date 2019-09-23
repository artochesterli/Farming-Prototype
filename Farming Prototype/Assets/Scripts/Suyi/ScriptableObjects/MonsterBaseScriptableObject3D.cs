using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterBaseScriptableObject3D : ScriptableObject
{
    public Sprite InBagSprite;
    public Mesh MonsterMesh;
    public Material MonsterMaterial;
    public float MovingSpeed = 1f;
    public float IdleMinDuration = 0.5f;
    public float IdleMaxDuration = 4f;

    public float IdleMoveMinDuration = 0.5f;
    public float IdleMoveMaxDuration = 1.5f;
    /// <summary>
	/// All Sights Combined
	/// </summary>
	public List<Sight> Sights;
    [Range(0f, 1f)]
    public float CaptureChance = 0.2f;
    public LayerMask Ground;
}
