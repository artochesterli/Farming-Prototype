using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SlimeActionState
{
    Normal,
    Generating
}

public class SlimeActionStateManager : MonoBehaviour, IComponentable
{
    public SlimeActionState CurrentState;
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

    public void SetActionState(SlimeActionState State)
    {
        CurrentState = State;
        if (CompareTag("Player"))
        {
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


}
