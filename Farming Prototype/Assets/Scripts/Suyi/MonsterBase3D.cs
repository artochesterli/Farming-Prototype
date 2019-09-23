using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterBase3D : MonoBehaviour, ICapturable, IUseable
{
    public MonsterBaseScriptableObject3D MonsterData;
    public bool Capturable { get => _capturable; set => _capturable = value; }
    private bool _capturable = true;

    protected GameObject _Player;

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

    public abstract bool OnHit(float chance);
    public abstract void OnCapture();
    public abstract void OnUse(PlayerController pc);
}
