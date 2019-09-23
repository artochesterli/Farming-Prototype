using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(PlayerInventory))]
public class PlayerController : MonoBehaviour
{
    public PlayerData PlayerData;
    public float CaptureCharge { get; set; }
    private FSM<PlayerController> _playerMovementFSM;
    private FSM<PlayerController> _playerActionFSM;
    private Rigidbody _rigidBody;
    private Slider _captureSlider;
    private PlayerInventory _inventory;

    private void Awake()
    {
        _playerMovementFSM = new FSM<PlayerController>(this);
        _playerActionFSM = new FSM<PlayerController>(this);
        _playerMovementFSM.TransitionTo<MovementIdleState>();
        _playerActionFSM.TransitionTo<ActionIdleState>();
        _rigidBody = GetComponent<Rigidbody>();
        _captureSlider = transform.Find("UICanvas").Find("CaptureCharge").GetComponent<Slider>();
        _inventory = GetComponent<PlayerInventory>();
    }

    void Update()
    {
        _playerMovementFSM.Update();
        _playerActionFSM.Update();
    }

    private void FixedUpdate()
    {
        _playerMovementFSM.FixedUpdate();
        _playerActionFSM.FixedUpdate();
    }

    public void OnImpact(KnockOff impact)
    {
        if (_playerMovementFSM.CurrentState.GetType().Equals(typeof(MovementKnockOffState))) return;
        _playerMovementFSM.TransitionTo<MovementKnockOffState>();
        Sequence seq = DOTween.Sequence();
        Vector3 _finalPos = transform.position + impact.Direction * impact.Force;
        _finalPos.y = transform.position.y;
        seq.Append(transform.DOMove(_finalPos, impact.Duration));
        seq.AppendCallback(() =>
        {
            _playerMovementFSM.TransitionTo<MovementIdleState>();
        });
    }

    private abstract class PlayerState : FSM<PlayerController>.State
    {
        protected float _horizontalAxis { get { return Input.GetAxis("Horizontal"); } }
        protected float _verticalAxis { get { return Input.GetAxis("Vertical"); } }
        protected bool _mouseLeftClick { get { return Input.GetMouseButtonDown(0); } }
        protected bool _mouseLeftClickUp { get { return Input.GetMouseButtonUp(0); } }
        protected bool _mouseRightClick { get { return Input.GetMouseButtonDown(1); } }
        protected bool _mouseRightClickUp { get { return Input.GetMouseButtonUp(1); } }
        protected int _inventoryKeyDown
        {
            get
            {
                for (int i = 1; i <= 7; i++)
                {
                    if (Input.GetKeyDown(KeyCode.Alpha0 + i)) return i;
                }
                return 0;
            }
        }
        // public override void OnEnter() { print(GetType().Name); }
    }

    private abstract class TransformMovementState : PlayerState { }
    private abstract class TransformActionState : PlayerState { }

    private abstract class PlayerMovementState : PlayerState { }

    private abstract class PlayerActionState : PlayerState { }

    private class MovementIdleState : PlayerMovementState
    {
        public override void OnEnter()
        {
            base.OnEnter();
            Context._rigidBody.velocity = Vector3.zero;
        }
        public override void Update()
        {
            base.Update();
            if (!Mathf.Approximately(_horizontalAxis, 0f) || !Mathf.Approximately(_verticalAxis, 0f))
            {
                TransitionTo<MovementRunState>();
                return;
            }
        }
    }

    private class MovementRunState : PlayerMovementState
    {
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (Input.GetAxisRaw("Horizontal") == 0f && Input.GetAxisRaw("Vertical") == 0f)
            {
                TransitionTo<MovementIdleState>();
                return;
            }
            Context._rigidBody.velocity = Context.PlayerData.MovingSpeed * new Vector3(_horizontalAxis, 0f, _verticalAxis).normalized;

            Vector3 relPos = Quaternion.AngleAxis(Mathf.Atan2(_horizontalAxis, _verticalAxis) * Mathf.Rad2Deg, Context.transform.up) * Vector3.forward;
            Quaternion rotation = Quaternion.LookRotation(relPos, Vector3.up);
            Quaternion tr = Quaternion.Slerp(Context.transform.rotation, rotation, Time.deltaTime * Context.PlayerData.RotationSpeed);
            Context.transform.rotation = tr;
        }
    }

    private class MovementThrowBallState : PlayerMovementState { }

    private class MovementKnockOffState : PlayerMovementState
    {
    }

    private class ActionIdleState : PlayerActionState
    {
        private float _captureTimer;
        private RaycastHit _hit;
        public override void OnEnter()
        {
            base.OnEnter();
            _captureTimer = Time.time + Context.PlayerData.CaptureDischargeInterval;
        }

        public override void Update()
        {
            base.Update();
            if (_mouseRightClick)
            {
                TransitionTo<ActionChargingState>();
                return;
            }

            if (_mouseLeftClick &&
                Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out _hit, Mathf.Infinity, Context.PlayerData.ThrowBallMouseCastLayer) &&
                Vector3.Distance(Context.transform.position, _hit.point) <= Context.PlayerData.ThrowBallRange)
            {
                TransitionTo<ActionThrowBallState>();
                Context._playerMovementFSM.TransitionTo<MovementThrowBallState>();
                return;
            }

            int ik = _inventoryKeyDown;
            if (ik != 0)
            {
                Context._inventory.Items[ik - 1].Useable.OnUse(Context);
            }

            if (_captureTimer < Time.time && Context.CaptureCharge >= 0f)
            {
                Context.CaptureCharge -= Time.deltaTime * Context.PlayerData.CaptureDischargeSpeed;
                Context._captureSlider.value = Context.CaptureCharge;
            }
        }
    }

    private class ActionChargingState : PlayerActionState
    {
        public override void Update()
        {
            base.Update();
            if (Context.CaptureCharge <= 1f)
            {
                Context.CaptureCharge += Time.deltaTime * Context.PlayerData.CaptureChargeSpeed;
                Context._captureSlider.value = Context.CaptureCharge;
            }
            if (_mouseRightClickUp)
            {
                TransitionTo<ActionIdleState>();
                return;
            }
        }
    }

    private class ActionThrowBallState : PlayerActionState
    {
        private float _throwTimer;
        private RaycastHit _hit;
        public override void OnEnter()
        {
            base.OnEnter();
            _throwTimer = Time.time + Context.PlayerData.ThrowBallInterval;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out _hit, Mathf.Infinity, Context.PlayerData.ThrowBallMouseCastLayer);
        }

        public override void Update()
        {
            base.Update();
            if (_hit.collider == null)
            {
                TransitionTo<ActionIdleState>();
                Context._playerMovementFSM.TransitionTo<MovementIdleState>();
                return;
            }
            if (_throwTimer < Time.time)
            {

                GameObject throwball = Instantiate(Context.PlayerData.ThrowBallPrefab, Context.transform.position, Context.PlayerData.ThrowBallPrefab.transform.rotation);
                throwball.GetComponent<CaptureUtilityBase>().BoostChance = Context.CaptureCharge * 0.2f;
                throwball.GetComponent<CaptureUtilityBase>().OnOut(_hit.point);
                Context.CaptureCharge = 0f;
                Context._captureSlider.value = Context.CaptureCharge;
                Context._playerMovementFSM.TransitionTo<MovementIdleState>();
                TransitionTo<ActionIdleState>();
                return;
            }
        }
    }
}
