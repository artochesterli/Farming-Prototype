using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedManager : MonoBehaviour
{
    public Vector3 SelfSpeed;
    public Vector3 ForcedSpeed;
    public Vector3 SelfSpeedDirection;

    public float CurrentNormalSpeed;
    public float CurrentStickySlowDownSpeed;
    public float CurrentPushFieldSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        SetSpeed();
    }

    public void SetSpeed()
    {

        SelfSpeed = SelfSpeedDirection * GetCurrentSelfSpeedValue();
        ForcedSpeed = GetComponent<DetectPushField>().Direction * GetCurrentForceSpeedValue();

        GetComponent<Rigidbody>().velocity = SelfSpeed + ForcedSpeed;

        if (!CompareTag("Player"))
        {
            //Debug.Log(ForcedSpeed);
        }
    }

    public float GetCurrentSelfSpeedValue()
    {
        if (GetComponent<DetectStickyField>().InStickyField)
        {
            return CurrentStickySlowDownSpeed;
        }
        else
        {
            return CurrentNormalSpeed;
        }
    }

    public float GetCurrentForceSpeedValue()
    {
        if (GetComponent<DetectPushField>().InPushField)
        {
            return CurrentPushFieldSpeed;
        }
        else
        {
            return 0;
        }
    }

    public void SetSpeedData(float Normal, float Sticky, float Push)
    {
        CurrentNormalSpeed = Normal;
        CurrentStickySlowDownSpeed = Sticky;
        CurrentPushFieldSpeed = Push;
    }
}
