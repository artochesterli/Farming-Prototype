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

