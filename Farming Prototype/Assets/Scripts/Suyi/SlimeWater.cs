using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SlimeWater : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		// TODO: Trip Player
		if (collision.CompareTag("Player"))
		{
			throw new System.NotImplementedException("Should Implement Tripping Player Here");
		}
	}
}
