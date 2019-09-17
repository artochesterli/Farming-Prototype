using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public abstract class MonsterBase : MonoBehaviour
{
	public MonsterBaseScriptableObject _MonsterData;
	protected Animator _Animator;

	public MonsterDirection _MonsterDirection { get; protected set; }

	protected virtual void Awake()
	{
		_MonsterDirection = MonsterDirection.Down;
		_Animator = GetComponent<Animator>();
	}

	protected virtual void _BasicMovement()
	{
		transform.Translate(_MonsterData.MovingSpeed * Time.deltaTime * Utility.DegreeToVector2(90f * (int)_MonsterDirection));
	}

	protected virtual void _BasicMovement(float customSpeed)
	{
		transform.Translate(customSpeed * Time.deltaTime * Utility.DegreeToVector2(90f * (int)_MonsterDirection));
	}

	protected void _TurnDirection(MonsterDirection _dir)
	{
		_MonsterDirection = _dir;
		switch (_dir)
		{
			case MonsterDirection.Down:
				_Animator.SetBool("MoveDown", true);
				break;
			case MonsterDirection.Up:
				_Animator.SetBool("MoveUp", true);
				break;
			case MonsterDirection.Left:
				_Animator.SetBool("MoveLeft", true);
				break;
			case MonsterDirection.Right:
				_Animator.SetBool("MoveRight", true);
				break;
		}
	}

	public abstract bool OnCaptured();

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		foreach (Sight s in _MonsterData.Sights)
		{

		}
	}
}