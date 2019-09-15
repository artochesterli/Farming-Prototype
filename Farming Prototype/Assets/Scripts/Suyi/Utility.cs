using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public static class Utility
{
	public static Vector2 RadianToVector2(float radian)
	{
		return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
	}

	public static Vector2 DegreeToVector2(float degree)
	{
		return RadianToVector2(degree * Mathf.Deg2Rad);
	}
}

[Serializable]
public class Sight
{
	public float Radius = 1f;
	/// <summary>
	/// Sight Angle
	/// </summary>
	public float Angle = 60f;
	/// <summary>
	/// Start Angle Sweeps Right
	/// </summary>
	public float StartAngle = 90f;
}