using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CaptureBasicBall : CaptureUtilityBase
{
    private Tween _flyTween;
    private CaptureBallData _cpd;

    private void Awake()
    {
        _cpd = CaptureUtilData as CaptureBallData;
    }

    public override void OnOut(Vector3 EndPos)
    {
        _flyTween = GetComponent<Rigidbody>().DOJump(EndPos, _cpd.ThrowBallPower, 1, _cpd.ThrowBallToDestinationDuration).SetEase(Ease.OutQuad);
    }

    protected override void OnHitTarget(ICapturable monster)
    {
        base.OnHitTarget(monster);
        _flyTween.Kill();
        monster.OnHit(_cpd.CaptureChance);
    }

    protected override void OnMissTarget()
    {
        base.OnMissTarget();
        _flyTween.Kill();
    }
}
