using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event { }

public class SwtichAction : Event
{
    public ActionType CurrentType;
    public SwtichAction(ActionType type)
    {
        CurrentType = type;
    }
}

public class SwtichTool : Event
{
    public Tool CurrentTool;
    public SwtichTool(Tool tool)
    {
        CurrentTool = tool;
    }
}

public class SetSeedNumber : Event
{
    public int Num;
    public SetSeedNumber(int num)
    {
        Num = num;
    }
}

public class SetMonsterNum : Event
{
    public int Num;
    public SetMonsterNum(int num)
    {
        Num = num;
    }
}

public class VitalityChange : Event
{
    public float CurrentVitality;
    public VitalityChange(float value)
    {
        CurrentVitality = value;
    }
}

public class CallCharacterActionStateChange : Event
{
    public CharacterActionState CurrentState;
    public CallCharacterActionStateChange(CharacterActionState state)
    {
        CurrentState = state;
    }
}

public class CallCharacterMovementStateChange : Event
{
    public CharacterMovementState CurrentState;
    public CallCharacterMovementStateChange(CharacterMovementState state)
    {
        CurrentState = state;
    }
}

public class CallSetCharacterSpeed : Event
{
    public float NormalSpeed;
    public float StickySlowDownSpeed;
    public CallSetCharacterSpeed(float normalSpeed,float stickySlowDownSpeed)
    {
        NormalSpeed = normalSpeed;
        StickySlowDownSpeed = stickySlowDownSpeed;
    }
}

public class CallSlimeActionStateChange : Event
{
    public SlimeActionState CurrentState;
    public CallSlimeActionStateChange(SlimeActionState state)
    {
        CurrentState = state;
    }
}
