using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DryadAbility : MonoBehaviour
{
    private float DodgeTimeCount;
    private int ThornCount;
    private Vector3 Direction;
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

            Direction = transform.rotation * Vector3.right;

            StartDodge();
        }
    }

    private bool InputAvailable()
    {
        return Input.GetMouseButtonDown(0);
    }

    public void StartDodge()
    {
        var DryadData = GetComponent<DryadData>();
        float Speed = DryadData.DodgeDis / DryadData.DodgeTime;
        GetComponent<Rigidbody>().velocity = Direction * Speed;
        StartPos = transform.position;

        GetComponent<DryadActionStateManager>().SetActionState(DryadActionState.Dodging);
    }

    private void Dodging()
    {
        var DryadData = GetComponent<DryadData>();

        float ThornInterval = DryadData.DodgeTime / (DryadData.ThornNumber + 1);
        float ThornDisInterval = DryadData.DodgeDis / (DryadData.ThornNumber + 1);

        if (GetComponent<DryadActionStateManager>().CurrentState == DryadActionState.Dodging)
        {
            if (GetComponent<CharacterMovementStateManager>().MovementState == CharacterMovementState.StickySlowDown)
            {
                ResetDodge();
            }

            DodgeTimeCount += Time.deltaTime;
            if (DodgeTimeCount >= ThornCount * ThornInterval && ThornCount<DryadData.ThornNumber)
            {
                Instantiate(Resources.Load("Chang/Prefabs/Static/Thorn"), StartPos + Direction*ThornDisInterval*ThornCount, Quaternion.Euler(0, 0, 0));
                ThornCount++;
            }
            if (DodgeTimeCount >= DryadData.DodgeTime)
            {
                ResetDodge();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(GetComponent<DryadActionStateManager>().CurrentState == DryadActionState.Dodging)
        {
            ResetDodge();
        }
    }

    private void ResetDodge()
    {
        DodgeTimeCount = 0;
        ThornCount = 0;
        GetComponent<Rigidbody>().velocity = Vector3.zero;

        GetComponent<DryadActionStateManager>().SetActionState(DryadActionState.Normal);
    }

    private void FixedUpdate()
    {
        Dodging();
    }
}
