using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum CharacterMovementState
{
    Normal,
    StickySlowDown
}

public class CharacterMovementStateManager : MonoBehaviour
{
    public CharacterMovementState MovementState;

    public float CurrentNormalSpeed;
    public float CurrentStickySlowDownSpeed;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.instance.AddHandler<CallCharacterMovementStateChange>(OnCallMovementStateChange);
        EventManager.instance.AddHandler<CallSetCharacterSpeed>(OnCallSetCharacterSpeed);

        var CharacterData = GetComponent<CharacterData>();
        EventManager.instance.Fire(new CallSetCharacterSpeed(CharacterData.NormalSpeed, CharacterData.StickySlowDownSpeed));

    }

    private void OnDestroy()
    {
        EventManager.instance.RemoveHandler<CallCharacterMovementStateChange>(OnCallMovementStateChange);
        EventManager.instance.RemoveHandler<CallSetCharacterSpeed>(OnCallSetCharacterSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   
    private void OnCallMovementStateChange(CallCharacterMovementStateChange Change)
    {
        MovementState = Change.CurrentState;
        switch (Change.CurrentState)
        {
            case CharacterMovementState.Normal:
                GetComponent<CharacterMove>().Speed = CurrentNormalSpeed;
                break;
            case CharacterMovementState.StickySlowDown:
                GetComponent<CharacterMove>().Speed = CurrentStickySlowDownSpeed;
                break;
        }
    }

    private void OnCallSetCharacterSpeed(CallSetCharacterSpeed Set)
    {
        CurrentNormalSpeed = Set.NormalSpeed;
        CurrentStickySlowDownSpeed = Set.StickySlowDownSpeed;
        switch (MovementState)
        {
            case CharacterMovementState.Normal:
                GetComponent<CharacterMove>().Speed = CurrentNormalSpeed;
                break;
            case CharacterMovementState.StickySlowDown:
                GetComponent<CharacterMove>().Speed = CurrentStickySlowDownSpeed;
                break;
        }
    }
}
