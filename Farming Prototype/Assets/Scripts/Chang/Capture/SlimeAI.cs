using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAI : MonsterBase3D
{
    private FSM<SlimeAI> SlimeAIFSM;

    public override void OnCapture()
    {
        Destroy(gameObject);
    }

    public override bool OnHit(float chance)
    {
        float rand = Random.Range(0f, 1f);
        // If capture success
        if (rand < chance * _CaptureChance)
        {
            OnCapture();
            return true;
        }
        else
        {
            Capturable = true;
        }
        return false;
    }

    public override void ControllableSetup()
    {
        gameObject.AddComponent<SlimeMonsterData>();
        gameObject.AddComponent<SlimeAbility>();
        gameObject.AddComponent<SlimeActionStateManager>();
        gameObject.AddComponent<SpeedManager>();
        gameObject.AddComponent<CharacterMove>();
        var tempData = GetComponent<SlimeMonsterData>();
        GetComponent<SpeedManager>().SetSpeedData(tempData.NormalSpeed, tempData.StickySlowDownSpeed, tempData.PushSpeed);
    }

    // Start is called before the first frame update
    void Start()
    {
        SlimeAIFSM = new FSM<SlimeAI>(this);
        if (!CompareTag("Player"))
            SlimeAIFSM.TransitionTo<SlimeAIState_Generate>();
        else
            SlimeAIFSM.TransitionTo<SlimeControlledSState>();

        _monsterTransform = new SlimeTransform(MonsterData as SlimeData);
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

public class SlimeControlledSState : FSM<SlimeAI>.State
{ }

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
