using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMoveInfo : MonoBehaviour
{
	public Vector2 Speed;

	public bool HitRight;
	public float RightDis;
	public GameObject Right;

	public bool HitLeft;
	public float LeftDis;
	public GameObject Left;

	public bool HitTop;
	public float TopDis;
	public GameObject Top;

	public bool HitBottom;
	public float BottomDis;
	public GameObject Bottom;

	public float ColliderWidth;
	public float ColliderHeight;
	public float DetectMargin;
	public Vector2 PivotPoint;


	/*public float BodyLowLength;
    public float BodyHighLength;
    public float BodyLeftLength;
    public float BodyRightLength;*/

	public LayerMask layermask;

	private const float DetectDis = 1;
	private const float HitMargin = 0.01f;


	// Update is called once per frame
	void LateUpdate()
	{
		CheckBottomDis();
		CheckLeftDis();
		CheckRightDis();
		CheckTopDis();

		//RectifySpeed();
		Move();

	}



	/*private void RectifySpeed()
    {
        if (HitRight && Speed.x > 0 || HitLeft && Speed.x < 0)
        {
            Speed.x = 0;
        }
        if (HitTop && Speed.y > 0 || HitGround && Speed.y < 0)
        {
            Speed.y = 0;
        }
    }*/



	public void Move()
	{
        if (GetComponent<CharacterStateManager>().State == CharacterState.Fall)
        {
            Speed = Vector2.zero;
        }
		Vector2 temp = Speed;
        

		if (TopDis < temp.y * Time.deltaTime)
		{
			temp.y = TopDis / Time.deltaTime;
			Speed.y = 0;
		}

		if (BottomDis < -temp.y * Time.deltaTime)
		{
			temp.y = -BottomDis / Time.deltaTime;
			Speed.y = 0;
		}

		if (LeftDis < -temp.x * Time.deltaTime)
		{
			temp.x = -LeftDis / Time.deltaTime;
			Speed = Vector2.zero;
		}

		if (RightDis < temp.x * Time.deltaTime)
		{
			temp.x = RightDis / Time.deltaTime;
			Speed = Vector2.zero;
		}

		transform.position += (Vector3)temp * Time.deltaTime;
	}

	public void CheckBottomDis()
	{
		Vector2 OriPoint = (Vector2)transform.position + PivotPoint;
		RaycastHit2D hit1 = Physics2D.Raycast(OriPoint + Vector2.right * (ColliderWidth / 2 - DetectMargin), Vector2.down, DetectDis, layermask);
		RaycastHit2D hit2 = Physics2D.Raycast(OriPoint + Vector2.left * (ColliderWidth / 2 - DetectMargin), Vector2.down, DetectDis, layermask);
		if (hit1 && hit2)
		{
			if (Mathf.Abs(hit1.point.y - OriPoint.y) < Mathf.Abs(hit2.point.y - OriPoint.y))
			{
				BottomDis = Mathf.Abs(hit1.point.y - OriPoint.y) - ColliderHeight / 2;
				Bottom = hit1.collider.gameObject;
			}
			else
			{
				BottomDis = Mathf.Abs(hit2.point.y - OriPoint.y) - ColliderHeight / 2;
				Bottom = hit2.collider.gameObject;
			}
		}
		else if (hit1)
		{
			BottomDis = Mathf.Abs(hit1.point.y - OriPoint.y) - ColliderHeight / 2;
			Bottom = hit1.collider.gameObject;
		}
		else if (hit2)
		{
			BottomDis = Mathf.Abs(hit2.point.y - OriPoint.y) - ColliderHeight / 2;
			Bottom = hit2.collider.gameObject;
		}
		else
		{
			BottomDis = System.Int32.MaxValue;
			Bottom = null;
		}

	}

	public void CheckTopDis()
	{
		Vector2 OriPoint = (Vector2)transform.position + PivotPoint;
		RaycastHit2D hit1 = Physics2D.Raycast(OriPoint + Vector2.right * (ColliderWidth / 2 - DetectMargin), Vector2.up, DetectDis, layermask);
		RaycastHit2D hit2 = Physics2D.Raycast(OriPoint + Vector2.left * (ColliderWidth / 2 - DetectMargin), Vector2.up, DetectDis, layermask);

		if (hit1 && hit2)
		{
			if (Mathf.Abs(hit1.point.y - OriPoint.y) < Mathf.Abs(hit2.point.y - OriPoint.y))
			{
				TopDis = Mathf.Abs(hit1.point.y - OriPoint.y) - ColliderHeight / 2;
				Top = hit1.collider.gameObject;
			}
			else
			{
				TopDis = Mathf.Abs(hit2.point.y - OriPoint.y) - ColliderHeight / 2;
				Top = hit2.collider.gameObject;
			}

		}
		else if (hit1)
		{
			TopDis = Mathf.Abs(hit1.point.y - OriPoint.y) - ColliderHeight / 2;
			Top = hit1.collider.gameObject;
		}
		else if (hit2)
		{
			TopDis = Mathf.Abs(hit2.point.y - OriPoint.y) - ColliderHeight / 2;
			Top = hit2.collider.gameObject;
		}
		else
		{
			TopDis = System.Int32.MaxValue;
			Top = null;
		}
	}

	public void CheckLeftDis()
	{
		Vector2 OriPoint = (Vector2)transform.position + PivotPoint;
		RaycastHit2D hit1 = Physics2D.Raycast(OriPoint + Vector2.up * (ColliderHeight / 2 - DetectMargin), Vector2.left, DetectDis, layermask);
		RaycastHit2D hit2 = Physics2D.Raycast(OriPoint + Vector2.down * (ColliderHeight / 2 - DetectMargin), Vector2.left, DetectDis, layermask);
		if (hit1 && hit2)
		{
			if (Mathf.Abs(hit1.point.x - OriPoint.x) < Mathf.Abs(hit2.point.x - OriPoint.x))
			{
				LeftDis = Mathf.Abs(hit1.point.x - OriPoint.x) - ColliderWidth / 2;
				Left = hit1.collider.gameObject;
			}
			else
			{
				LeftDis = Mathf.Abs(hit2.point.x - OriPoint.x) - ColliderWidth / 2;
				Left = hit2.collider.gameObject;
			}
		}
		else if (hit1)
		{
			LeftDis = Mathf.Abs(hit1.point.x - OriPoint.x) - ColliderWidth / 2;
			Left = hit1.collider.gameObject;
		}
		else if (hit2)
		{
			LeftDis = Mathf.Abs(hit2.point.x - OriPoint.x) - ColliderWidth / 2;
			Left = hit2.collider.gameObject;
		}
		else
		{
			LeftDis = System.Int32.MaxValue;
			Left = null;
		}
	}

	public void CheckRightDis()
	{
		Vector2 OriPoint = (Vector2)transform.position + PivotPoint;
		RaycastHit2D hit1 = Physics2D.Raycast(OriPoint + Vector2.up * (ColliderHeight / 2 - DetectMargin), Vector2.right, DetectDis, layermask);
		RaycastHit2D hit2 = Physics2D.Raycast(OriPoint + Vector2.down * (ColliderHeight / 2 - DetectMargin), Vector2.right, DetectDis, layermask);
		if (hit1 && hit2)
		{
			if (Mathf.Abs(hit1.point.x - OriPoint.x) < Mathf.Abs(hit2.point.x - OriPoint.x))
			{
				RightDis = Mathf.Abs(hit1.point.x - OriPoint.x) - ColliderWidth / 2;
				Right = hit1.collider.gameObject;
			}
			else
			{
				RightDis = Mathf.Abs(hit2.point.x - OriPoint.x) - ColliderWidth / 2;
				Right = hit2.collider.gameObject;
			}
		}
		else if (hit1)
		{
			RightDis = Mathf.Abs(hit1.point.x - OriPoint.x) - ColliderWidth / 2;
			Right = hit1.collider.gameObject;
		}
		else if (hit2)
		{
			RightDis = Mathf.Abs(hit2.point.x - OriPoint.x) - ColliderWidth / 2;
			Right = hit2.collider.gameObject;
		}
		else
		{
			RightDis = System.Int32.MaxValue;
			Right = null;
		}
	}
}
