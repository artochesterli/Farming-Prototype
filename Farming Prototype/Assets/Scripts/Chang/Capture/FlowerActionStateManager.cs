using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum FlowerActionState
{
    Normal,
    Blowing
}

public class FlowerActionStateManager : MonoBehaviour
{
    public FlowerActionState CurrentState;
    
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

    public void SetActionState(FlowerActionState State)
    {
        CurrentState = State;
        if (CompareTag("Player"))
        {
            switch (CurrentState)
            {
                case FlowerActionState.Normal:
                    GetComponent<CharacterMove>().enabled = true;
                    break;
                case FlowerActionState.Blowing:
                    GetComponent<CharacterMove>().enabled = false;
                    break;
            }
        }
    }
}
