using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

	private abstract class AlertState : SlimeState
	{
		private GameObject _player;

		public override void OnEnter()
		{
			base.OnEnter();
			_player = GameObject.FindGameObjectWithTag("Player");
			Debug.Assert(_player != null, "Player Object Not Found");
		}

		public override void Update()
		{
			base.Update();

		}

		private bool HasPlayerInFront()
		{
			// TODO
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

		public override void OnEnter()
		{
			base.OnEnter();
			Array values = Enum.GetValues(typeof(MonsterDirection));
			System.Random rand = new System.Random();
			MonsterDirection randomDirection = (MonsterDirection)values.GetValue(rand.Next(values.Length));
			Context._MonsterDirection = randomDirection;
			switch (Context._MonsterDirection)
			{
				case MonsterDirection.Down:
					Context._Animator.SetBool("MoveDown", true);
					break;
				case MonsterDirection.Up:
					Context._Animator.SetBool("MoveUp", true);
					break;
				case MonsterDirection.Left:
					Context._Animator.SetBool("MoveLeft", true);
					break;
				case MonsterDirection.Right:
					Context._Animator.SetBool("MoveRight", true);
					break;
			}
			_moveTimer = Time.time + UnityEngine.Random.Range(_SlimeData.IdleMoveMinDuration, _SlimeData.IdleMoveMaxDuration);
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
				Context._BaseMovement();
			}
		}

		public override void OnExit()
		{
			base.OnExit();
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
