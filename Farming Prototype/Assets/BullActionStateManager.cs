using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BullActionState
{
    Normal,
    Charging
}

public class BullActionStateManager : MonoBehaviour
{
    public BullActionState CurrentState;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnDestroy()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetActionState(BullActionState State)
    {
        CurrentState = State;
        if (CompareTag("Player"))
        {
            switch (CurrentState)
            {
                case BullActionState.Normal:
                    GetComponent<CharacterMove>().enabled = true;
                    break;
                case BullActionState.Charging:
                    GetComponent<CharacterMove>().enabled = false;
                    break;
            }
        }
    }
}
