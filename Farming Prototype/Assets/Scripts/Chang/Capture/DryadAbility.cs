using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DryadAbility : MonoBehaviour
{
    private float DodgeTimeCount;
    private Vector3 Direction;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        Dodging();
    }

    private void CheckInput()
    {
        if(InputAvailable()&& CompareTag("Player") && GetComponent<DryadActionStateManager>().CurrentState == DryadActionState.Normal)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                Direction = hit.point - hit.point.y * Vector3.up - (transform.position - transform.position.y * Vector3.up);
                Direction.Normalize();
            }

            StartDodge();
        }
    }

    private bool InputAvailable()
    {
        return Input.GetMouseButtonDown(0);
    }

    private void StartDodge()
    {
        if (CompareTag("Player"))
        {
            EventManager.instance.Fire(new CallDryadActionStateChange(DryadActionState.Dodging));
        }
    }

    private void Dodging()
    {
        var DryadData = GetComponent<DryadData>();
        float Speed = DryadData.DodgeDis / DryadData.DodgeTime;
        if(GetComponent<DryadActionStateManager>().CurrentState == DryadActionState.Dodging)
        {
            DodgeTimeCount += Time.deltaTime;
            GetComponent<Rigidbody>().velocity = Direction * Speed;
            
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
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        if (CompareTag("Player"))
        {
            EventManager.instance.Fire(new CallDryadActionStateChange(DryadActionState.Normal));
        }
    }
}
