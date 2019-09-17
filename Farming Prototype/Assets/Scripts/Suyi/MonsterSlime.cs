using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class MonsterSlime : MonsterBase
{
	private FSM<MonsterSlime> _slimeFSM;

	protected override void Awake()
	{
		base.Awake();
		_slimeFSM = new FSM<MonsterSlime>(this);
		_slimeFSM.TransitionTo<SlimeIdleState>();
	}

	private void Update()
	{
		_slimeFSM.Update();
	}

	IEnumerator _emitSlimeWater(float time)
	{
		SlimeData sd = _MonsterData as SlimeData;
		while (true)
		{
			GameObject go = Instantiate(sd.SlimeWaterPrefab, transform.position, Quaternion.identity);
			Destroy(go, (sd.SlimeWaterExistanceDuration));
			yield return new WaitForSeconds(time);
		}
	}

	public override bool OnCaptured()
	{
		if (_slimeFSM.CurrentState.GetType().BaseType.Equals(typeof(AlertState)))
		{
			_slimeFSM.TransitionTo<CapturedIdleState>();
			return true;
		}
		return false;
	}

	private abstract class SlimeState : FSM<MonsterSlime>.State
	{
		protected SlimeData _SlimeData;
		public override void Init()
		{
			base.Init();
			_SlimeData = Context._MonsterData as SlimeData;
			Debug.Assert(_SlimeData != null, "Slime Data not Found");
		}

		public override void OnEnter()
		{
			base.OnEnter();
			print(GetType().Name);
		}
	}

	private abstract class WildState : SlimeState { }

	private abstract class CapturedState : SlimeState { }

	private class CapturedIdleState : CapturedState
	{
		private float _idleTimer;
		public override void OnEnter()
		{
			base.OnEnter();
			Context._Animator.SetBool("Idle", true);
			_idleTimer = Time.time + UnityEngine.Random.Range(_SlimeData.IdleMinDuration, _SlimeData.IdleMaxDuration);
		}

		public override void Update()
		{
			base.Update();
			if (_idleTimer < Time.time)
			{
				TransitionTo<SlimeIdleMoveState>();
				return;
			}
		}

		public override void OnExit()
		{
			base.OnExit();
			Context._Animator.SetBool("Idle", false);
		}
	}

	private class CapturedIdleMoveState : CapturedState
	{
		private float _moveTimer;
		private IEnumerator _emitSlime;

		public override void OnEnter()
		{
			base.OnEnter();
			Array values = Enum.GetValues(typeof(MonsterDirection));
			System.Random rand = new System.Random();
			MonsterDirection randomDirection = (MonsterDirection)values.GetValue(rand.Next(values.Length));
			Context._TurnDirection(randomDirection);
			_moveTimer = Time.time + UnityEngine.Random.Range(_SlimeData.IdleMoveMinDuration, _SlimeData.IdleMoveMaxDuration);
			_emitSlime = Context._emitSlimeWater(_SlimeData.SlimeWaterEmitInterval);
			Context.StartCoroutine(_emitSlime);
		}

		public override void Update()
		{
			base.Update();
			if (_moveTimer < Time.time)
			{
				TransitionTo<SlimeIdleState>();
				return;
			}
			else
			{
				Context._BasicMovement();
			}
		}

		public override void OnExit()
		{
			base.OnExit();
			Context.StopCoroutine(_emitSlime);
			switch (Context._MonsterDirection)
			{
				case MonsterDirection.Down:
					Context._Animator.SetBool("MoveDown", false);
					break;
				case MonsterDirection.Up:
					Context._Animator.SetBool("MoveUp", false);
					break;
				case MonsterDirection.Left:
					Context._Animator.SetBool("MoveLeft", false);
					break;
				case MonsterDirection.Right:
					Context._Animator.SetBool("MoveRight", false);
					break;
			}
		}
	}

	private abstract class AlertState : WildState
	{
		private GameObject _player;

		public override void Init()
		{
			base.Init();
			_player = GameObject.FindGameObjectWithTag("Player");
			Debug.Assert(_player != null, "Player Object Not Found");
		}

		public override void Update()
		{
			base.Update();
			if (_hasPlayerInFront())
			{
				TransitionTo<SlimeSurprisedState>();
				return;
			}
		}

		private bool _hasPlayerInFront()
		{
			// 1. Player is in range
			// 2. Player is in angle
			foreach (Sight s in _SlimeData.Sights)
			{
				if (Vector2.Distance(_player.transform.position, Context.transform.position) <= s.Radius
					 && Vector2.Angle(Utility.DegreeToVector2(90f * (int)Context._MonsterDirection + s.AngleOffset), _player.transform.position - Context.transform.position) < s.Angle / 2f)
				{
					return true;
				}
			}
			return false;
		}
	}

	/// <summary>
	/// Slime defaultly switch between Idle State
	/// and Wonder Walk Randomly State
	/// </summary>
	private class SlimeIdleState : AlertState
	{
		private float _idleTimer;
		public override void OnEnter()
		{
			base.OnEnter();
			Context._Animator.SetBool("Idle", true);
			_idleTimer = Time.time + UnityEngine.Random.Range(_SlimeData.IdleMinDuration, _SlimeData.IdleMaxDuration);
		}

		public override void Update()
		{
			base.Update();
			if (_idleTimer < Time.time)
			{
				TransitionTo<SlimeIdleMoveState>();
				return;
			}
		}

		public override void OnExit()
		{
			base.OnExit();
			Context._Animator.SetBool("Idle", false);
		}
	}

	private class SlimeIdleMoveState : AlertState
	{
		private float _moveTimer;
		private IEnumerator _emitSlime;

		public override void OnEnter()
		{
			base.OnEnter();
			Array values = Enum.GetValues(typeof(MonsterDirection));
			System.Random rand = new System.Random();
			MonsterDirection randomDirection = (MonsterDirection)values.GetValue(rand.Next(values.Length));
			Context._TurnDirection(randomDirection);
			_moveTimer = Time.time + UnityEngine.Random.Range(_SlimeData.IdleMoveMinDuration, _SlimeData.IdleMoveMaxDuration);
			_emitSlime = Context._emitSlimeWater(_SlimeData.SlimeWaterEmitInterval);
			Context.StartCoroutine(_emitSlime);
		}

		public override void Update()
		{
			base.Update();
			if (_moveTimer < Time.time)
			{
				TransitionTo<SlimeIdleState>();
				return;
			}
			else
			{
				Context._BasicMovement();
			}
		}

		public override void OnExit()
		{
			base.OnExit();
			Context.StopCoroutine(_emitSlime);
			switch (Context._MonsterDirection)
			{
				case MonsterDirection.Down:
					Context._Animator.SetBool("MoveDown", false);
					break;
				case MonsterDirection.Up:
					Context._Animator.SetBool("MoveUp", false);
					break;
				case MonsterDirection.Left:
					Context._Animator.SetBool("MoveLeft", false);
					break;
				case MonsterDirection.Right:
					Context._Animator.SetBool("MoveRight", false);
					break;
			}
		}
	}

	private class SlimeSurprisedState : WildState
	{
		public override void OnEnter()
		{
			base.OnEnter();
			Context._Animator.SetBool("Idle", true);
			GameObject exclamationPoint = GameObject.Instantiate(_SlimeData.ExclamationPrefab, Context.transform);
			Sequence seq = DOTween.Sequence();
			seq.AppendInterval(_SlimeData.ExclamationDuration);
			seq.AppendCallback(() =>
			{
				Destroy(exclamationPoint);
				TransitionTo<SlimeRunAwayState>();
			});
		}

		public override void OnExit()
		{
			base.OnExit();
			Context._Animator.SetBool("Idle", false);
		}
	}

	private class SlimeRunAwayState : WildState
	{
		private float _moveTimer;
		private IEnumerator _emitSlime;

		public override void OnEnter()
		{
			base.OnEnter();
			_moveTimer = Time.time + UnityEngine.Random.Range(_SlimeData.FleeMinDuration, _SlimeData.FleeMaxDuration);
			// Run Towards the opposite direciton of current direction
			Array values = Enum.GetValues(typeof(MonsterDirection));
			System.Random rand = new System.Random();
			MonsterDirection randomDirection = (MonsterDirection)values.GetValue(rand.Next(values.Length));
			while (randomDirection == Context._MonsterDirection)
			{
				randomDirection = (MonsterDirection)values.GetValue(rand.Next(values.Length));
			}
			Context._TurnDirection(randomDirection);
			_emitSlime = Context._emitSlimeWater(_SlimeData.SlimeWaterSuprisedEmitInterval);
			Context.StartCoroutine(_emitSlime);
		}

		public override void Update()
		{
			base.Update();
			if (_moveTimer < Time.time)
			{
				TransitionTo<SlimeIdleState>();
				return;
			}
			else
			{
				Context._BasicMovement(_SlimeData.FleeMovingSpeed);
			}
		}

		public override void OnExit()
		{
			base.OnExit();
			Context.StopCoroutine(_emitSlime);
			switch (Context._MonsterDirection)
			{
				case MonsterDirection.Down:
					Context._Animator.SetBool("MoveDown", false);
					break;
				case MonsterDirection.Up:
					Context._Animator.SetBool("MoveUp", false);
					break;
				case MonsterDirection.Left:
					Context._Animator.SetBool("MoveLeft", false);
					break;
				case MonsterDirection.Right:
					Context._Animator.SetBool("MoveRight", false);
					break;
			}
		}
	}
}
