using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BullAbility : MonoBehaviour
{
    private Vector3 CurrentDirection;
    private Vector3 TargetDirection;

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
        BullActionState state = GetComponent<BullActionStateManager>().CurrentState;
        var BullData = GetComponent<BullData>();
        if (InputActive() && CompareTag("Player"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                Vector3 Direction = hit.point - hit.point.y * Vector3.up - (transform.position - transform.position.y * Vector3.up);
                Direction.Normalize();

                Chase(Direction);
            }

            if (state == BullActionState.Normal)
            {
                GetComponent<BullActionStateManager>().SetActionState(BullActionState.Charging);
            }
        }

        if (!InputActive())
        {
            if(state == BullActionState.Charging)
            {
                GetComponent<BullActionStateManager>().SetActionState(BullActionState.Normal);
            }
        }
    }

    public void Chase(Vector3 Direction)
    {
        if (GetComponent<BullActionStateManager>().CurrentState==BullActionState.Normal)
        {
            CurrentDirection = Direction;
            TargetDirection = Direction;
        }
        else
        {
            TargetDirection = Direction;

            Turn();
        }
    }

    private void Turn()
    {
        var BullData = GetComponent<BullData>();

        float Angle = Vector3.SignedAngle(CurrentDirection, TargetDirection, Vector3.up);
        if (Angle > 0)
        {
            CurrentDirection = Quaternion.Euler(0, BullData.ChargeDirectionTurnSpeed * Time.deltaTime, 0) * CurrentDirection;
        }
        else
        {
            CurrentDirection = Quaternion.Euler(0, -BullData.ChargeDirectionTurnSpeed * Time.deltaTime, 0) * CurrentDirection;
        }

        if (Vector3.Dot(CurrentDirection, TargetDirection) > 0)
        {
            GetComponent<Rigidbody>().velocity = BullData.ChargeSpeed * CurrentDirection;
        }
        else
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

        float DirectionAngle = Vector3.SignedAngle(Vector3.right, CurrentDirection, Vector3.up);
        transform.rotation = Quaternion.Euler(0, DirectionAngle, 0);
    }

    private bool InputActive()
    {
        return Input.GetMouseButton(0);
    }
}
