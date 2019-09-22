using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlowerAbility : MonoBehaviour
{
    public GameObject PushFieldCanvas;

    public float DetectionAngleInterval = 5;

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
        CheckObjectInside();
        
    }

    private void CheckInput()
    {
        FlowerActionState state = GetComponent<FlowerActionStateManager>().CurrentState;
        var FlowerData = GetComponent<FlowerData>();
        if (InputActive() && CompareTag("Player"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                Vector3 Direction = hit.point - hit.point.y * Vector3.up - (transform.position - transform.position.y * Vector3.up);
                Direction.Normalize();
                Follow(Direction);
            }

            if (state == FlowerActionState.Normal)
            {
                GetComponent<FlowerActionStateManager>().SetActionState(FlowerActionState.Blowing);
            }
        }

        if (!InputActive())
        {
            if (state == FlowerActionState.Blowing)
            {
                DeactivateField();
                GetComponent<FlowerActionStateManager>().SetActionState(FlowerActionState.Normal);
            }
        }
    }

    public void ActivateField()
    {
        PushFieldCanvas.transform.GetChild(0).GetComponent<Image>().enabled = true;
    }

    public void DeactivateField()
    {
        PushFieldCanvas.transform.GetChild(0).GetComponent<Image>().enabled = false;
    }

    public void Follow(Vector3 Direction)
    {
        if (GetComponent<FlowerActionStateManager>().CurrentState == FlowerActionState.Normal)
        {
            ActivateField();
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
        var FlowerData = GetComponent<FlowerData>();

        float Angle = Vector3.SignedAngle(CurrentDirection, TargetDirection, Vector3.up);
        if (Angle > 0)
        {
            CurrentDirection = Quaternion.Euler(0, FlowerData.PushFieldTurnSpeed * Time.deltaTime, 0) * CurrentDirection;
        }
        else
        {
            CurrentDirection = Quaternion.Euler(0, -FlowerData.PushFieldTurnSpeed * Time.deltaTime, 0) * CurrentDirection;
        }

        float DirectionAngle = Vector3.SignedAngle(Vector3.right, CurrentDirection, Vector3.up);
        transform.rotation = Quaternion.Euler(0, DirectionAngle, 0);
    }


    private bool InputActive()
    {
        return Input.GetMouseButton(0);
    }

    private void CheckObjectInside()
    {
        var FlowerData = GetComponent<FlowerData>();
        float Angle = 0;
        Vector3 v = Quaternion.Euler(0, -FlowerData.PushFieldAngle / 2, 0) * CurrentDirection;
        Vector3 Init = v;

        List<GameObject> AllObjectInField = new List<GameObject>();


        while (Angle < FlowerData.PushFieldAngle)
        {
            RaycastHit[] Allhits = Physics.RaycastAll(transform.position, v, FlowerData.PushFieldRadius);
            Angle += DetectionAngleInterval;
            v = Quaternion.Euler(0, Angle, 0) * Init;
            for(int i = 0; i < Allhits.Length; i++)
            {
                GameObject obj = Allhits[i].collider.gameObject;
                if (obj.CompareTag("Player"))
                {
                    AllObjectInField.Add(obj);
                }
                
            }
        }
    }


}
