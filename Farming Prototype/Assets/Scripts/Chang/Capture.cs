using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Capture : MonoBehaviour
{
	public float CaptureDis;
	public GameObject CaptureNet;
    public GameObject AllCapturedMonster;

	private bool CaptureNetActive;
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		CheckState();
		CheckInput();
	}

	private void CheckInput()
	{
		if (InputAvailable())
		{
			var ItemHoldingInfo = GetComponent<ItemHoldingInfo>();
			var CharacterMoveInfo = GetComponent<CharacterMoveInfo>();

			if (CaptureNetActive)
			{
				Collider2D[] Colliders = Physics2D.OverlapCircleAll(CaptureNet.transform.position, CaptureDis);

				List<GameObject> Captureables = new List<GameObject>();
				for (int i = 0; i < Colliders.Length; i++)
				{
					if (Colliders[i].gameObject.layer == 9 && Colliders[i].gameObject.GetComponent<MonsterBase>().OnCaptured())
					{
						Captureables.Add(Colliders[i].gameObject);
					}
				}

				if (Captureables.Count > 0)
				{
					var VitalityManager = GetComponent<VitalityManager>();
					EventManager.instance.Fire(new VitalityChange(VitalityManager.Vitality -= VitalityManager.CaptureCost));
				}

				for (int i = 0; i < Captureables.Count; i++)
				{
					ItemHoldingInfo.MonsterCaptured[0].number++;
					EventManager.instance.Fire(new SetMonsterNum(ItemHoldingInfo.MonsterCaptured[0].number));
                    Captureables[i].transform.parent = AllCapturedMonster.transform;
                    Captureables[i].SetActive(false);
                    Debug.Log(Captureables[i]);
					//Destroy(Captureables[i]);
				}

			}
		}
	}

	private void CheckState()
	{
		ActionType type = GetComponent<ActionSelection>().CurrentType;
		var ItemHoldingInfo = GetComponent<ItemHoldingInfo>();
		System.Type ToolType = ItemHoldingInfo.ToolList[ItemHoldingInfo.ToolHoldingIndex].GetType();
		if (type == ActionType.Tool && ToolType == typeof(CaptureNet))
		{
			CaptureNetActive = true;
			CaptureNet.SetActive(true);
		}
		else
		{
			CaptureNetActive = false;
			CaptureNet.SetActive(false);
		}
	}

	private bool InputAvailable()
	{
		return Input.GetMouseButtonDown(0);
	}
}
