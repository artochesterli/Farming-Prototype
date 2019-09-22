using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterBase3D : MonoBehaviour, ICapturable, IUseable
{
    public MonsterBaseScriptableObject3D MonsterData;
    public bool Capturable { get => _capturable; set => _capturable = value; }
    private bool _capturable = true;


    public abstract bool OnHit(float chance);
    public abstract void OnCapture();
    public abstract void OnUse(PlayerController pc);
}
