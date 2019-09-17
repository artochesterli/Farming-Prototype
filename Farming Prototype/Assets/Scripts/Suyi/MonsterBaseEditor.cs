﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MonsterSlime))]
public class MonsterBaseEditor : Editor
{
	private MonsterBase mb;

	private void OnEnable()
	{
		mb = this.target as MonsterBase;
	}

	private void OnSceneGUI()
	{
		Handles.color = Color.red;
		foreach (Sight s in mb._MonsterData.Sights)
		{
			Handles.DrawSolidArc(mb.transform.position, -mb.transform.forward, Utility.DegreeToVector2(90f * (int)mb._MonsterDirection), s.Angle / 2f, s.Radius);
			Handles.DrawSolidArc(mb.transform.position, mb.transform.forward, Utility.DegreeToVector2(90f * (int)mb._MonsterDirection), s.Angle / 2f, s.Radius);
		}
	}
}