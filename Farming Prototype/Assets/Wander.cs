using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : MonoBehaviour
{
    public Vector2 StayTimeMinMax;
    public float WanderRadius;

    public Vector3 Center;

    private float WanderTimeCount;
    private float StayTimeCount;
    private float CurrentWanderTime;
    private float CurrentStayTime;

    private bool Moving;
    // Start is called before the first frame update
    void Start()
    {
        CurrentStayTime = Random.Range(StayTimeMinMax.x, StayTimeMinMax.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (Moving)
        {
            WanderTimeCount += Time.deltaTime;
            if (WanderTimeCount >= CurrentWanderTime)
            {
                CurrentStayTime = Random.Range(StayTimeMinMax.x, StayTimeMinMax.y);
                StayTimeCount = 0;
                Moving = false;
                GetComponent<SpeedManager>().SelfSpeedDirection = Vector3.zero;
            }
        }
        else
        {
            StayTimeCount += Time.deltaTime;
            if (StayTimeCount >= CurrentStayTime)
            {
                Vector2 v = Random.insideUnitCircle * WanderRadius;
                Vector3 Target = Center + new Vector3(v.x,0,v.y);

                WanderTimeCount = 0;
                Moving = true;

                Vector3 Dir = Target - transform.position;
                Dir.y = 0;

                GetComponent<SpeedManager>().SelfSpeedDirection = Dir.normalized;
                transform.rotation = Quaternion.Euler(0, Vector3.SignedAngle(Vector3.right, Dir,Vector3.up), 0);
                CurrentWanderTime = Dir.magnitude / GetComponent<SpeedManager>().GetCurrentSelfSpeedValue();
            }
        }
    }

    public void OnEnterWander()
    {
        Center = transform.position;
        GetComponent<SpeedManager>().SelfSpeedDirection = Vector3.zero;
    }

    public void OnExitWander()
    {
        Moving = false;
        WanderTimeCount = 0;
        StayTimeCount = 0;
        CurrentStayTime = Random.Range(StayTimeMinMax.x, StayTimeMinMax.y);
        GetComponent<SpeedManager>().SelfSpeedDirection = Vector3.zero;
    }


}
