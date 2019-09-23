using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterFlower : MonsterBase3D
{
    private FSM<MonsterFlower> _flowerFSM;
    private FlowerMonsterData _flowerData;
    private ParticleSystem _pushField;

    protected override void Awake()
    {
        base.Awake();
        _flowerFSM = new FSM<MonsterFlower>(this);
        _flowerFSM.TransitionTo<FlowerIdleState>();
        _monsterTransform = new FlowerTransform();
        _pushField = transform.Find("PushField").GetComponent<ParticleSystem>();
        _flowerData = MonsterData as FlowerMonsterData;
        var main = _pushField.main;
        main.startLifetime = _flowerData.ShootRadius / 45f;
        var shape = _pushField.shape;
        shape.angle = _flowerData.ShootAngle / 2f;
    }

    private void Update()
    {
        _flowerFSM.Update();
    }

    private abstract class FlowerState : FSM<MonsterFlower>.State
    {

        public override void OnEnter()
        {
            base.OnEnter();
            print(GetType().Name);
        }
    }

    private abstract class FlowerNormalState : FlowerState
    {
        public override void Update()
        {
            base.Update();
            if (Context._HasPlayerInSight())
            {
                TransitionTo<FlowerAttackState>();
            }
        }
    }

    private abstract class FlowerEngagedState : FlowerState { }

    private class FlowerAttackState : FlowerEngagedState
    {
        private float _returnToNormalTimer;
        private bool _playerInAttackRange()
        {
            if (Vector3.Distance(Context._Player.transform.position, Context.transform.position) <= Context._flowerData.ShootRadius
            && Vector3.Angle(Context.transform.forward, Context._Player.transform.position - Context.transform.position) < Context._flowerData.ShootAngle / 2f)
            {
                return true;
            }
            return false;
        }
        public override void OnEnter()
        {
            base.OnEnter();
            Context._pushField.gameObject.SetActive(true);
        }

        public override void Update()
        {
            base.Update();
            Vector3 _dir = (Context._Player.transform.position - Context.transform.position).normalized;
            float angle = Vector3.SignedAngle(Context.transform.forward, _dir, Vector3.up);
            _dir = Quaternion.Euler(0f, (angle > 0 ? 1f : -1f) * Context._flowerData.TurnTendency * Time.deltaTime, 0f) * Context.transform.forward;
            Context.transform.eulerAngles = Vector3.up * Mathf.Atan2(_dir.x, _dir.z) * Mathf.Rad2Deg;

            if (_playerInAttackRange())
            {
                Context._Player.GetComponent<PlayerController>().OnImpact(new KnockOff(
                    (Context._Player.transform.position - Context.transform.position).normalized,
                    Context._flowerData.PushForce,
                    Context._flowerData.PushDuration)
                );
            }

            if (!Context._HasPlayerInSight())
            {
                _returnToNormalTimer += Time.deltaTime;
            }
            else
            {
                if (_returnToNormalTimer >= 0f)
                    _returnToNormalTimer -= Time.deltaTime;
            }
            if (_returnToNormalTimer >= Context._flowerData.ResetTime)
            {
                _returnToNormalTimer = 0f;
                TransitionTo<FlowerIdleState>();
                return;
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            Context._pushField.gameObject.SetActive(false);
        }
    }

    private class FlowerIdleState : FlowerNormalState
    {

    }

    public override void OnCapture()
    {
        throw new System.NotImplementedException();
    }

    public override bool OnHit(float chance)
    {
        throw new System.NotImplementedException();
    }

}
