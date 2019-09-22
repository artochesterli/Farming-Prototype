using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BullData : MonoBehaviour
{
    public float NormalSpeed = 6;
    public float StickySlowDownSpeed = 3;
    public float PushSpeed = 8;

    public float ChargeSpeed = 12;
    public float ChargeDirectionTurnSpeed = 90;
    public float ChargeCoolDown = 5;

    // Start is called before the first frame update
    void Start()
    {
        if (!CompareTag("Player"))
        {
            GetComponent<SpeedManager>().SetSpeedData(NormalSpeed, StickySlowDownSpeed, PushSpeed);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
