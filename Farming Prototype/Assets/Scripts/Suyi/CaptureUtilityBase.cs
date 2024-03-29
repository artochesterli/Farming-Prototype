﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CaptureUtilityBase : MonoBehaviour
{
    public CaptureUtilScriptableObject CaptureUtilData;
    public float BoostChance { get; set; }
    protected bool _CanCapture = true;

    public virtual void OnOut(Vector3 EndPos) { }

    /// <summary>
    /// When Capture Utility Hit the Capturable Target
    /// </summary>
    /// <param name="monster"></param>
    protected virtual void OnHitTarget(ICapturable monster)
    {
        _CanCapture = false;
    }

    /// <summary>
    /// When Capture Utility Hit something that is not a 
    /// Capturable Target
    /// </summary>
    protected virtual void OnMissTarget()
    {
        _CanCapture = false;
    }

    protected virtual void OnCollisionEnter(Collision other)
    {
        if (!_CanCapture) return;
        if (other.collider.CompareTag("Player")) return;

        if (other.collider.GetComponent<ICapturable>() != null)
        {
            OnHitTarget(other.collider.GetComponent<ICapturable>());
        }
        else
        {
            OnMissTarget();
        }
    }
}
