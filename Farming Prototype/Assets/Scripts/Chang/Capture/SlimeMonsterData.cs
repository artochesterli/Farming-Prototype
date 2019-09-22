using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeMonsterData : MonoBehaviour
{
    public float NormalSpeed = 6;
    public float StickySlowDownSpeed = 6;
    public float PushSpeed = 6;

    public float FieldInitSize = 1;
    public float FieldMaxSize = 4;
    public float FieldGenerationTime = 2;
    public float FieldGenerationCoolDown = 3;

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
