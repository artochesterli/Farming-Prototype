﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DryadAI : MonsterBase3D
{
    public Vector3 FleeDir;

    private FSM<DryadAI> DryadAIFSM;

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
        gameObject.AddComponent<DryadData>();
        gameObject.AddComponent<DryadAbility>();
        gameObject.AddComponent<DryadActionStateManager>();
        gameObject.AddComponent<SpeedManager>();
        gameObject.AddComponent<CharacterMove>();
        var tempData = GetComponent<DryadData>();
        GetComponent<SpeedManager>().SetSpeedData(tempData.NormalSpeed, tempData.StickySlowDownSpeed, tempData.PushSpeed);
    }

    // Start is called before the first frame update
    void Start()
    {
        DryadAIFSM = new FSM<DryadAI>(this);
        if (!CompareTag("Player"))
            DryadAIFSM.TransitionTo<DryadAIState_Wander>();
        else
            DryadAIFSM.TransitionTo<DryadControlledState>();
        _monsterTransform = new DryadTransform(MonsterData as MonsterDryadData);

    }

    // Update is called once per frame
    void Update()
    {
        DryadAIFSM.Update();
    }
}

public abstract class DryadAIState : FSM<DryadAI>.State
{
    protected GameObject DryadEntity;

    public override void Init()
    {
        base.Init();
        DryadEntity = Context.gameObject;
    }

}

public class DryadControlledState : FSM<DryadAI>.State
{ }

public class DryadAIState_Wander : DryadAIState
{

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("Wander");
        DryadEntity.GetComponent<Wander>().OnEnterWander();
        DryadEntity.GetComponent<Wander>().enabled = true;

    }

    public override void Update()
    {
        base.Update();
        var DetectDanger = DryadEntity.GetComponent<DetectDanger>();
        if (DetectDanger.Detect(DetectDanger.SaveDetectAngle, DetectDanger.AlertDis))
        {
            if (DetectDanger.DetectedBall)
            {
                Vector3 v = DetectDanger.DetectedBall.transform.position - DryadEntity.transform.position;
                v.y = 0;
                float Dis = v.magnitude;
                if (Dis < DetectDanger.ResponseDis)
                {
                    Context.FleeDir = -v.normalized;
                    TransitionTo<DryadAIState_Dodge>();
                }
                else
                {
                    TransitionTo<DryadAIState_Alert>();
                }
            }
            else
            {
                Vector3 v = DetectDanger.DetectedPlayer.transform.position - DryadEntity.transform.position;
                v.y = 0;
                float Dis = v.magnitude;

                if (Dis < DetectDanger.ResponseDis)
                {
                    Context.FleeDir = -v.normalized;
                    TransitionTo<DryadAIState_Flee>();
                }
                else
                {
                    TransitionTo<DryadAIState_Alert>();
                }
            }


        }

    }

    public override void OnExit()
    {
        base.OnExit();
        DryadEntity.GetComponent<Wander>().enabled = false;
        DryadEntity.GetComponent<Wander>().OnExitWander();
    }
}

public class DryadAIState_Alert : DryadAIState
{
    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("Alert");
    }

    public override void Update()
    {
        base.Update();
        DryadEntity.GetComponent<SpeedManager>().SelfSpeedDirection = Vector3.zero;
        var DetectDanger = DryadEntity.GetComponent<DetectDanger>();

        if (DetectDanger.Detect(DetectDanger.SaveDetectAngle, DetectDanger.AlertDis))
        {
            if (DetectDanger.DetectedBall)
            {
                Vector3 v = DetectDanger.DetectedBall.transform.position - DryadEntity.transform.position;
                v.y = 0;
                float Dis = v.magnitude;
                DryadEntity.transform.rotation = Quaternion.Euler(0, Vector3.SignedAngle(Vector3.right, v, Vector3.up), 0);

                if (Dis < DetectDanger.ResponseDis)
                {
                    Context.FleeDir = -v.normalized;
                    TransitionTo<DryadAIState_Dodge>();
                }
            }
            else
            {
                Vector3 v = DetectDanger.DetectedPlayer.transform.position - DryadEntity.transform.position;
                v.y = 0;
                float Dis = v.magnitude;
                DryadEntity.transform.rotation = Quaternion.Euler(0, Vector3.SignedAngle(Vector3.right, v, Vector3.up), 0);

                if (Dis < DetectDanger.ResponseDis)
                {
                    Context.FleeDir = -v.normalized;
                    TransitionTo<DryadAIState_Flee>();
                }
            }
        }
        else
        {
            TransitionTo<DryadAIState_Wander>();
        }
    }
}

public class DryadAIState_Dodge : DryadAIState
{
    public override void OnEnter()
    {
        base.OnEnter();
        DryadEntity.GetComponent<DryadAbility>().StartDodge(Context.FleeDir);
    }

    public override void Update()
    {
        base.Update();
        if (DryadEntity.GetComponent<DryadActionStateManager>().CurrentState == DryadActionState.Normal)
        {
            TransitionTo<DryadAIState_Flee>();
        }
    }

}

public class DryadAIState_Flee : DryadAIState
{
    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("Flee");
        DryadEntity.GetComponent<SpeedManager>().SelfSpeedDirection = Context.FleeDir;
        DryadEntity.transform.rotation = Quaternion.Euler(0, Vector3.SignedAngle(Vector3.right, Context.FleeDir, Vector3.up), 0);
    }

    public override void Update()
    {
        base.Update();
        var DetectDanger = DryadEntity.GetComponent<DetectDanger>();
        if (!DetectDanger.Detect(DetectDanger.InDangerDetectAngle, DetectDanger.SaveDis))
        {
            TransitionTo<DryadAIState_Wander>();
        }
    }
}
