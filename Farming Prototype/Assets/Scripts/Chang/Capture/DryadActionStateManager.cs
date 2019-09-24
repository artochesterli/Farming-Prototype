using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DryadActionState
{
    Normal,
    Dodging
}

public class DryadActionStateManager : MonoBehaviour, IComponentable
{
    public DryadActionState CurrentState;
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

    public void SetActionState(DryadActionState State)
    {
        CurrentState = State;
        if (CompareTag("Player"))
        {
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
}
