using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DryadCapturable : ICapturable
{
    public void OnCapture()
    {

    }

    public void OnHit(float Chance)
    {

    }
}

public class DryadAbility : MonoBehaviour
{
    private float DodgeTimeCount;
    private int ThornCount;
    public Vector3 Direction;
    private Vector3 StartPos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        
    }

    private void FixedUpdate()
    {
        Dodging();
    }

    private void CheckInput()
    {
        if(InputAvailable()&& CompareTag("Player") && GetComponent<DryadActionStateManager>().CurrentState == DryadActionState.Normal)
        {
            /*Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                Direction = hit.point - hit.point.y * Vector3.up - (transform.position - transform.position.y * Vector3.up);
                Direction.Normalize();
            }*/

            StartDodge(transform.rotation * Vector3.right);
        }
    }

    private bool InputAvailable()
    {
        return Input.GetMouseButtonDown(0);
    }

    public void StartDodge(Vector3 Dir)
    {
        Direction = Dir;
        transform.rotation = Quaternion.Euler(0, Vector3.SignedAngle(Vector3.right, Direction, Vector3.up), 0);
        var DryadData = GetComponent<DryadData>();
        float Speed = DryadData.DodgeDis / DryadData.DodgeTime;
        GetComponent<SpeedManager>().CurrentNormalSpeed = Speed;
        GetComponent<SpeedManager>().SelfSpeedDirection = Direction;

        StartPos = transform.position;

        GetComponent<DryadActionStateManager>().SetActionState(DryadActionState.Dodging);
    }

    public void StopDodge()
    {
        var DryadData = GetComponent<DryadData>();

        DodgeTimeCount = 0;
        ThornCount = 0;

        GetComponent<SpeedManager>().CurrentNormalSpeed = DryadData.NormalSpeed;

        GetComponent<DryadActionStateManager>().SetActionState(DryadActionState.Normal);
    }


    private void Dodging()
    {
        var DryadData = GetComponent<DryadData>();

        float ThornInterval = DryadData.DodgeTime / (DryadData.ThornNumber + 1);
        float ThornDisInterval = DryadData.DodgeDis / (DryadData.ThornNumber + 1);

        if (GetComponent<DryadActionStateManager>().CurrentState == DryadActionState.Dodging)
        {
            if (GetComponent<DetectStickyField>().InStickyField)
            {
                StopDodge();
            }

            DodgeTimeCount += Time.deltaTime;
            if (DodgeTimeCount >= ThornCount * ThornInterval && ThornCount<DryadData.ThornNumber)
            {
                Instantiate(Resources.Load("Chang/Prefabs/Static/Thorn"), StartPos + Direction*ThornDisInterval*ThornCount, Quaternion.Euler(0, 0, 0));
                ThornCount++;
            }
            if (DodgeTimeCount >= DryadData.DodgeTime)
            {
                StopDodge();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(GetComponent<DryadActionStateManager>().CurrentState == DryadActionState.Dodging)
        {
            StopDodge();
        }
    }

    
}
