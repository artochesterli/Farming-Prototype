using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterBase3D : MonoBehaviour, ICapturable, IComponentable
{
    public MonsterBaseScriptableObject3D MonsterData;
    public bool Capturable { get => _capturable; set => _capturable = value; }
    protected bool _capturable = true;
    protected virtual float _CaptureChance
    {
        get
        {
            return MonsterData.CaptureChance / SpeedMultiplier;
        }
    }
    protected GameObject _Player;
    protected float SpeedMultiplier = 1f;
    public MonsterTransform _monsterTransform;

    protected virtual void Awake()
    {
        _Player = GameObject.FindGameObjectWithTag("Player");
        Debug.Assert(_Player != null);
    }

    protected virtual bool _HasPlayerInSight()
    {
        foreach (Sight s in MonsterData.Sights)
        {
            if (Vector3.Distance(_Player.transform.position, transform.position) <= s.Radius
                 && Vector3.Angle(transform.forward + Vector3.forward * s.AngleOffset, _Player.transform.position - transform.position) < s.Angle / 2f)
            {
                return true;
            }
        }
        return false;
    }

    public virtual void ControllableSetup()
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="chance">The Chance of the Ball</param>
    /// <returns></returns>
    public abstract bool OnHit(float chance);
    public abstract void OnCapture();
}
