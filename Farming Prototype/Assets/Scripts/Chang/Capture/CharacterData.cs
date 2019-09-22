using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData : MonoBehaviour
{
    public float NormalSpeed;
    public float StickySlowDownSpeed;
    public float PushSpeed;
    public float EnergyChargeSpeed;
    public float EnergyDissipateSpeed;
    public float EnergyHoldTime;
    public float CaptureBallShootPreparingTime;
    public float CaptureBallInitOffset;
    public float CaptureBallHeight;
    public float CaptureBallSpeed;
    public float CaptureBallSize;
    public float CaptureBallMaximalDis;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpeedManager>().SetSpeedData(NormalSpeed, StickySlowDownSpeed, PushSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
