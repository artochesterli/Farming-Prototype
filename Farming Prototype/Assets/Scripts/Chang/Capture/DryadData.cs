using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DryadData : MonoBehaviour
{
    public float NormalSpeed = 12;
    public float StickySlowDownSpeed = 1;
    public float PushSpeed = 12;

    public float DodgeDis = 6;
    public float DodgeTime = 0.2f;
    public float DodgeCoolDown = 2;

    public int ThornNumber = 3;

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
