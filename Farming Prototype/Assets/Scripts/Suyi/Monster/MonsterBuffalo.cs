using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBuffalo : MonsterBase3D
{
    private FSM<MonsterBuffalo> _buffaloFSM;

    private void Start()
    {
        _buffaloFSM = new FSM<MonsterBuffalo>(this);
        if (!CompareTag("Player"))
            _buffaloFSM.TransitionTo<BuffaloIdleState>();
        else
            _buffaloFSM.TransitionTo<ControllableIdleState>();
        _monsterTransform = new BuffaloTransform(MonsterData as BuffaloData);
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

    private void _abilityCharge(Vector3 _dir)
    {
        BuffaloData _buffaloData = MonsterData as BuffaloData;
        float angle = Vector3.SignedAngle(transform.forward, _dir, Vector3.up);
        _dir = Quaternion.Euler(0f, (angle > 0 ? 1f : -1f) * _buffaloData.TurnTendency * Time.deltaTime, 0f) * transform.forward;
        transform.eulerAngles = Vector3.up * Mathf.Atan2(_dir.x, _dir.z) * Mathf.Rad2Deg;
        GetComponent<Rigidbody>().velocity = _dir * _buffaloData.ChargeSpeed * SpeedMultiplier;
    }

    private abstract class ControllableBuffaloState : BuffaloState
    {
        protected float _horizontalAxis { get { return Input.GetAxis("Horizontal"); } }
        protected float _verticalAxis { get { return Input.GetAxis("Vertical"); } }
        protected bool _mouseLeftClickDown { get { return Input.GetMouseButtonDown(0); } }
        protected bool _mouseLeftClick { get { return Input.GetMouseButton(0); } }
        protected bool _mouseLeftClickUp { get { return Input.GetMouseButtonUp(0); } }

    }

    private class ControllableIdleState : ControllableBuffaloState
    {
        public override void OnEnter()
        {
            base.OnEnter();
            _rb.velocity = Vector3.zero;
        }

        public override void Update()
        {
            base.Update();
            if (!Mathf.Approximately(_horizontalAxis, 0f) || !Mathf.Approximately(_verticalAxis, 0f))
            {
                TransitionTo<ControllableMoveState>();
                return;
            }
            if (_mouseLeftClick)
            {
                TransitionTo<ControllableChargeState>();
                return;
            }
        }
    }

    private class ControllableMoveState : ControllableBuffaloState
    {
        public override void Update()
        {
            base.Update();
            if (Input.GetAxisRaw("Horizontal") == 0f && Input.GetAxisRaw("Vertical") == 0f)
            {
                TransitionTo<ControllableIdleState>();
                return;
            }
            if (_mouseLeftClick)
            {
                TransitionTo<ControllableChargeState>();
                return;
            }
            _rb.velocity = _buffaloData.MovingSpeed * new Vector3(_horizontalAxis, 0f, _verticalAxis).normalized * Context.SpeedMultiplier;

            Vector3 relPos = Quaternion.AngleAxis(Mathf.Atan2(_horizontalAxis, _verticalAxis) * Mathf.Rad2Deg, Context.transform.up) * Vector3.forward;
            Quaternion rotation = Quaternion.LookRotation(relPos, Vector3.up);
            Context.transform.rotation = rotation;
        }
    }

    private class ControllableChargeState : ControllableBuffaloState
    {
        public override void Update()
        {
            base.Update();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _buffaloData.Ground))
            {
                Vector3 dir = hit.point;
                dir.y = Context.transform.position.y;
                dir = (dir - Context.transform.position).normalized;
                Context._abilityCharge(dir);
            }
            if (_mouseLeftClickUp)
            {
                TransitionTo<ControllableIdleState>();
                return;
            }
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

        public override void OnEnter()
        {
            base.OnEnter();
            print(GetType().Name);
        }
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
                Context._abilityCharge(_dir);
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
