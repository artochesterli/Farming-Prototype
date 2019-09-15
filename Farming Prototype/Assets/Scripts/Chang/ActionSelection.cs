using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionType
{
    Tool,
    Seed,
    Place
}

public class ActionSelection : MonoBehaviour
{
    public ActionType CurrentType;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
    }

    private void CheckInput()
    { 

        if (InputSeed())
        {
            CurrentType = ActionType.Seed;
        }

        if (InputPlace())
        {
            CurrentType = ActionType.Place;
        }

        if (InputTool())
        {
            CurrentType = ActionType.Tool;
        }

        EventManager.instance.Fire(new SwtichAction(CurrentType));
    }

    private bool InputTool()
    {
        return Input.GetKeyDown(KeyCode.Alpha1);
    }

    private bool InputSeed()
    {
        return Input.GetKeyDown(KeyCode.Alpha2);
    }
    private bool InputPlace()
    {
        return Input.GetKeyDown(KeyCode.Alpha3);
    }
}
