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
        if (!monster.Capturable) return;
        bool _captureSuccess = false;
        monster.Capturable = false;
        _flyTween.Kill();
        Sequence sequence = DOTween.Sequence();
        sequence.Append(GetComponent<Rigidbody>().DOMoveY(3f, 0.5f));
        sequence.AppendCallback(() =>
        {
            _captureSuccess = monster.OnHit(_cpd.CaptureChance + BoostChance);
            if (_captureSuccess)
            {
                GameObject Player = GameObject.FindGameObjectWithTag("Player");
                sequence.Append(GetComponent<Rigidbody>().DOMove(Player.transform.position, 0.5f)
                .OnComplete(() =>
                {
                    Player.GetComponent<PlayerInventory>().OnEnterBag(new Item(((MonsterBase3D)monster).MonsterData.InBagSprite,
                    ((MonsterBase3D)monster)._monsterTransform));
                    Destroy(gameObject);
                }));
            }
            else
            {
                Destroy(gameObject);
            }
        });

    }

    protected override void OnMissTarget()
    {
        base.OnMissTarget();
        _flyTween.Kill();
    }
}
