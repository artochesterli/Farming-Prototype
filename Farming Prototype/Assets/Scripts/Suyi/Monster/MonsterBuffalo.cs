using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBuffalo : MonsterBase3D
{
    private FSM<MonsterBuffalo> _buffaloFSM;

    protected override void Awake()
    {
        base.Awake();
        _buffaloFSM = new FSM<MonsterBuffalo>(this);
        _buffaloFSM.TransitionTo<BuffaloIdleState>();
        _monsterTransform = new BuffaloTransform();
    }

    private void Update()
    {
        _buffaloFSM.Update();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (_buffaloFSM.CurrentState.GetType().Equals(typeof(BuffaloChargeState)))
        {
            if (other.collider.CompareTag("Player"))
            {
                other.collider.GetComponent<PlayerController>().OnImpact(new KnockOff(
                    (other.transform.position - transform.position).normalized,
                    ((BuffaloData)MonsterData).PushForce,
                    ((BuffaloData)MonsterData).PushDuration
                ));
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("StickyField"))
        {
            SpeedMultiplier = 0.1f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("StickyField"))
        {
            SpeedMultiplier = 1f;
        }
    }
    private abstract class BuffaloState : FSM<MonsterBuffalo>.State
    {
        protected BuffaloData _buffaloData;
        protected Rigidbody _rb;
        public override void Init()
        {
            base.Init();
            _buffaloData = Context.MonsterData as BuffaloData;
            _rb = Context.GetComponent<Rigidbody>();
        }

        // public override void OnEnter()
        // {
        //     base.OnEnter();
        //     print(GetType().Name);
        // }
    }

    private abstract class BuffaloNormalState : BuffaloState
    {
        public override void Update()
        {
            base.Update();
            if (Context._HasPlayerInSight())
            {
                TransitionTo<BuffaloStareState>();
                return;
            }
        }
    }

    private abstract class BuffaloEnragedState : BuffaloState { }

    private class BuffaloStareState : BuffaloEnragedState
    {
        private float _stareTimer;

        public override void OnEnter()
        {
            base.OnEnter();
            _stareTimer = Time.time + Random.Range(_buffaloData.StareMinDuration, _buffaloData.StareMaxDuration) / Context.SpeedMultiplier;
        }

        public override void Update()
        {
            base.Update();
            Context.transform.LookAt(Context._Player.transform);
            if (_stareTimer < Time.time)
            {
                TransitionTo<BuffaloChargeState>();
                return;
            }
        }
    }

    private class BuffaloChargeState : BuffaloEnragedState
    {
        private Vector3 _finalPos;
        private Vector3 _velocityDir;
        private float _chargeTimer;

        public override void OnEnter()
        {
            base.OnEnter();
            _finalPos = new Vector3(Context._Player.transform.position.x, Context._Player.transform.position.y, Context._Player.transform.position.z);
            _chargeTimer = Vector3.Distance(Context.transform.position, _finalPos) / _buffaloData.ChargeSpeed + Time.time + _buffaloData.OverChargeTime;
            _velocityDir = (_finalPos - Context.transform.position).normalized;
            _rb.velocity = _velocityDir * _buffaloData.ChargeSpeed;
        }

        public override void Update()
        {
            base.Update();
            if (_chargeTimer < Time.time)
            {
                TransitionTo<BuffaloStareState>();
                return;
            }
            if (_chargeTimer - Time.time > _buffaloData.OverChargeTime)
            {
                Vector3 _dir = (Context._Player.transform.position - Context.transform.position).normalized;
                float angle = Vector3.SignedAngle(Context.transform.forward, _dir, Vector3.up);
                _dir = Quaternion.Euler(0f, (angle > 0 ? 1f : -1f) * _buffaloData.TurnTendency * Time.deltaTime, 0f) * Context.transform.forward;
                Context.transform.eulerAngles = Vector3.up * Mathf.Atan2(_dir.x, _dir.z) * Mathf.Rad2Deg;
                _rb.velocity = _dir * _buffaloData.ChargeSpeed * Context.SpeedMultiplier;
            }
        }
    }

    private class BuffaloIdleState : BuffaloNormalState
    {
        private float _idleTimer;
        public override void OnEnter()
        {
            base.OnEnter();
            _idleTimer = Time.time + Random.Range(_buffaloData.IdleMinDuration, _buffaloData.IdleMaxDuration);
        }

        public override void Update()
        {
            base.Update();
            if (_idleTimer < Time.time)
            {
                TransitionTo<BuffaloWonderState>();
                return;
            }
        }
    }

    private class BuffaloWonderState : BuffaloNormalState
    {
        private float _wonderTimer;
        private Vector3 _dir;
        public override void OnEnter()
        {
            base.OnEnter();
            _wonderTimer = Time.time + Random.Range(_buffaloData.IdleMoveMinDuration, _buffaloData.IdleMoveMaxDuration);
            _dir = new Vector3(Random.Range(0f, 1f), 0f, Random.Range(0f, 1f)).normalized;
            Context.transform.eulerAngles = Vector3.up * Mathf.Atan2(_dir.x, _dir.z) * Mathf.Rad2Deg;
        }

        public override void Update()
        {
            base.Update();
            if (_wonderTimer < Time.time)
            {
                TransitionTo<BuffaloIdleState>();
                return;
            }
            else
            {
                _rb.velocity = _dir * _buffaloData.MovingSpeed * Context.SpeedMultiplier;
            }
        }
    }

    public override bool OnHit(float chance)
    {
        float rand = Random.Range(0f, 1f);
        // If capture success
        if (rand < chance * _CaptureChance)
        {
            OnCapture();
            return true;
        }
        else
        {
            Capturable = true;
        }
        return false;
    }

    public override void OnCapture()
    {
        Destroy(gameObject);
    }
}
