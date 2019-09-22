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
    public CharacterActionState CurrentState;
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

    public void SetActionState(CharacterActionState State)
    {
        CurrentState = State;
        switch (CurrentState)
        {
            case CharacterActionState.Normal:
                GetComponent<CharacterMove>().enabled = true;
                break;
            case CharacterActionState.Charging:
                GetComponent<CharacterMove>().enabled = false;
                GetComponent<SpeedManager>().SelfSpeedDirection = Vector3.zero;
                break;
            case CharacterActionState.ShootPreparing:
                GetComponent<CharacterMove>().enabled = false;
                GetComponent<SpeedManager>().SelfSpeedDirection = Vector3.zero;
                break;
        }
    }
}
