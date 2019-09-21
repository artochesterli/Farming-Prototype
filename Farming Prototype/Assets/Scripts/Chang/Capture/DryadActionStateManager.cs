using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DryadActionState
{
    Normal,
    Dodging
}

public class DryadActionStateManager : MonoBehaviour
{
    public DryadActionState CurrentState;
    // Start is called before the first frame update
    void Start()
    {
        EventManager.instance.AddHandler<CallDryadActionStateChange>(OnCallActionStateChange);
    }

    private void OnDestroy()
    {
        EventManager.instance.RemoveHandler<CallDryadActionStateChange>(OnCallActionStateChange);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCallActionStateChange(CallDryadActionStateChange Change)
    {
        CurrentState = Change.CurrentState;
        switch (CurrentState)
        {
            case DryadActionState.Normal:
                GetComponent<CharacterMove>().enabled = true;
                break;
            case DryadActionState.Dodging:
                GetComponent<CharacterMove>().enabled = false;
                break;
        }
    }
}
