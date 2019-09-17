using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FieldState
{
	Dry,
	Wet
}

public class Field : MonoBehaviour
{
	public GameObject Seed;
	public GameObject Plant;
	public FieldState State;
	public float WetTime;

	public Color DryColor;
	public Color WetColor;

	private float WetTimeCount;
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		CheckState();
		SetAppearance();
	}

	private void SetAppearance()
	{
		switch (State)
		{
			case FieldState.Dry:
				GetComponent<SpriteRenderer>().color = DryColor;
				break;
			case FieldState.Wet:
				GetComponent<SpriteRenderer>().color = WetColor;
				break;
			default:
				break;
		}
	}

	private void CheckState()
	{
		if (State == FieldState.Wet)
		{
			WetTimeCount += Time.deltaTime;
			if (WetTimeCount >= WetTime)
			{
				WetTimeCount = 0;
				State = FieldState.Dry;
				if (Seed)
				{
					Plant = Seed.GetComponent<Seed>().Plant;
					Seed.GetComponent<Seed>().GrowPlant();
					Seed = null;
				}
			}
		}
	}

	public void WaterField()
	{
		State = FieldState.Wet;
	}

}
