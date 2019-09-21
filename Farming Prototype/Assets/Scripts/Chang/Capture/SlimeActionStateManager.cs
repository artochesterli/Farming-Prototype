using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SlimeActionState
{
    Normal,
    Generating
}

public class SlimeActionStateManager : MonoBehaviour
{
    public SlimeActionState CurrentState;
    // Start is called before the first frame update
    void Start()
    {
        EventManager.instance.AddHandler<CallSlimeActionStateChange>(OnCallActionStateChange);
    }

    private void OnDestroy()
    {
        EventManager.instance.RemoveHandler<CallSlimeActionStateChange>(OnCallActionStateChange);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCallActionStateChange(CallSlimeActionStateChange Change)
    {
        CurrentState = Change.CurrentState;
        switch (CurrentState)
        {
            case SlimeActionState.Normal:
                GetComponent<CharacterMove>().enabled = true;
                break;
            case SlimeActionState.Generating:
                GetComponent<CharacterMove>().enabled = false;
                break;
        }
    }

    
}
