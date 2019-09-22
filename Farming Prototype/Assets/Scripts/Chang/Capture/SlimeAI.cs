using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAI : MonoBehaviour
{ 
    private FSM<SlimeAI> SlimeAIFSM;



    // Start is called before the first frame update
    void Start()
    {
        SlimeAIFSM = new FSM<SlimeAI>(this);
        SlimeAIFSM.TransitionTo<SlimeAIState_Generate>();
    }

    // Update is called once per frame
    void Update()
    {
        SlimeAIFSM.Update();
    }
}

public abstract class SlimeAIState : FSM<SlimeAI>.State
{
    protected GameObject SlimeEntity;

    public override void Init()
    {
        base.Init();
        SlimeEntity = Context.gameObject;
    }
}

public class SlimeAIState_Generate : SlimeAIState
{
    public override void OnEnter()
    {
        base.OnEnter();
        SlimeEntity.GetComponent<SlimeAbility>().GenerateStickyField();
    }

    public override void Update()
    {
        base.Update();
        if (SlimeEntity.GetComponent<SlimeActionStateManager>().CurrentState == SlimeActionState.Normal)
        {
            TransitionTo<SlimeAIState_Wander>();
        }
    }
}

public class SlimeAIState_Wander : SlimeAIState
{
    private float TimeCount;

    public override void Init()
    {
        base.Init();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        SlimeEntity.GetComponent<Wander>().enabled = true;
        SlimeEntity.GetComponent<Wander>().Center = SlimeEntity.transform.position;
    }
}
