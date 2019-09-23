using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterFlower : MonsterBase3D
{
    private FSM<MonsterFlower> _flowerFSM;
    private FlowerMonsterData _flowerData;
    private ParticleSystem _pushField;

    private void Start()
    {
        _flowerFSM = new FSM<MonsterFlower>(this);
        if (!CompareTag("Player"))
            _flowerFSM.TransitionTo<FlowerIdleState>();
        else
            _flowerFSM.TransitionTo<ControllableFlowerIdleState>();
        _monsterTransform = new FlowerTransform(MonsterData as FlowerMonsterData);
        if (_pushField == null)
            _pushField = transform.Find("PushField").GetComponent<ParticleSystem>();
        _flowerData = MonsterData as FlowerMonsterData;
        var main = _pushField.main;
        main.startLifetime = _flowerData.ShootRadius / 45f;
        var shape = _pushField.shape;
        shape.angle = _flowerData.ShootAngle / 2f;
    }

    public override void ControllableSetup()
    {
        GameObject pf = null;
        Transform temp = transform.Find("PushField(Clone)");
        if (temp == null)
            pf = Instantiate(((FlowerMonsterData)MonsterData).PushFieldPrefab, transform, false);
        else
            pf = temp.gameObject;
        _pushField = pf.GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        _flowerFSM.Update();
    }

    private void _abilityFlower(Vector3 _dir)
    {
        float angle = Vector3.SignedAngle(transform.forward, _dir, Vector3.up);
        _dir = Quaternion.Euler(0f, (angle > 0 ? 1f : -1f) * _flowerData.TurnTendency * Time.deltaTime, 0f) * transform.forward;
        transform.eulerAngles = Vector3.up * Mathf.Atan2(_dir.x, _dir.z) * Mathf.Rad2Deg;
    }

    private abstract class FlowerState : FSM<MonsterFlower>.State
    {
        public override void OnEnter()
        {
            base.OnEnter();
            print(GetType().Name);
        }
    }

    private class ControllableFlowerIdleState : FlowerState
    {
        public override void Update()
        {
            base.Update();
            if (Input.GetMouseButtonDown(0))
            {
                TransitionTo<ControllableFlowerAttackState>();
                return;
            }
        }
    }

    private class ControllableFlowerAttackState : FlowerState
    {
        public override void OnEnter()
        {
            base.OnEnter();
            Context._pushField.gameObject.SetActive(true);
        }

        public override void OnExit()
        {
            base.OnExit();
            Context._pushField.gameObject.SetActive(false);
        }
        public override void Update()
        {
            base.Update();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, Context._flowerData.Ground))
            {
                Vector3 dir = hit.point;
                dir.y = Context.transform.position.y;
                dir = (dir - Context.transform.position).normalized;
                Context._abilityFlower(dir);
            }
            if (Input.GetMouseButtonUp(0))
            {
                TransitionTo<ControllableFlowerIdleState>();
                return;
            }
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
            Context._abilityFlower(_dir);
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
        Destroy(gameObject);
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

}
