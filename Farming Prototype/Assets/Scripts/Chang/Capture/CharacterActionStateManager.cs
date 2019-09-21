using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterActionState
{
    Normal,
    Charging,
    ShootPreparing
}

public class CharacterActionStateManager : MonoBehaviour
{
    public CharacterActionState ActionState;
    // Start is called before the first frame update
    void Start()
    {
        EventManager.instance.AddHandler<CallCharacterActionStateChange>(OnCallActionStateChange);
    }

    private void OnDestroy()
    {
        EventManager.instance.RemoveHandler<CallCharacterActionStateChange>(OnCallActionStateChange);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCallActionStateChange(CallCharacterActionStateChange Change)
    {
        ActionState = Change.CurrentState;
        switch (Change.CurrentState)
        {
            case CharacterActionState.Normal:
                GetComponent<CharacterMove>().enabled = true;
                break;
            case CharacterActionState.Charging:
                GetComponent<CharacterMove>().enabled = false;
                break;
            case CharacterActionState.ShootPreparing:
                GetComponent<CharacterMove>().enabled = false;
                break;
        }
    }
}
