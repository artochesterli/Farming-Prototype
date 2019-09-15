using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public abstract class MonsterBase : MonoBehaviour
{
	public MonsterBaseScriptableObject _MonsterData;
	protected Animator _Animator;
	protected enum MonsterDirection
	{
		Down,
		Up,
		Left,
		Right
	}

	protected MonsterDirection _MonsterDirection;

	protected virtual void Awake()
	{
		_MonsterDirection = MonsterDirection.Down;
		_Animator = GetComponent<Animator>();
	}

	protected virtual void _BaseMovement()
	{
		switch (_MonsterDirection)
		{
			case MonsterDirection.Down:
				transform.Translate(_MonsterData.MovingSpeed * Time.deltaTime * Vector3.down);
				break;
			case MonsterDirection.Up:
				transform.Translate(_MonsterData.MovingSpeed * Time.deltaTime * Vector3.up);
				break;
			case MonsterDirection.Right:
				transform.Translate(_MonsterData.MovingSpeed * Time.deltaTime * Vector3.right);
				break;
			case MonsterDirection.Left:
				transform.Translate(_MonsterData.MovingSpeed * Time.deltaTime * Vector3.left);
				break;
		}
		print("Base Movement");

	}
}
