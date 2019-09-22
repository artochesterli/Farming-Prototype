using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlowerCapturable : ICapturable
{
    public void OnCapture()
    {

    }

    public void OnHit(float Chance)
    {

    }
}

public class FlowerAbility : MonoBehaviour
{
    
    public GameObject PushFieldCanvas;

    public float DetectionAngleInterval = 5;
    public float CastHeight = 0.5f;

    private Vector3 CurrentDirection;
    private Vector3 TargetDirection;
    private List<GameObject> AllObjectsInField;
    // Start is called before the first frame update
    void Start()
    {
        AllObjectsInField = new List<GameObject>();
        if (!CompareTag("Player"))
        {
            ActivateField(Vector3.right);
        }
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

        if (CompareTag("Player"))
        {
            if (InputActive())
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    Vector3 Direction = hit.point - hit.point.y * Vector3.up - (transform.position - transform.position.y * Vector3.up);
                    Direction.Normalize();

                    if (state == FlowerActionState.Normal)
                    {
                        ActivateField(Direction);
                    }

                    Follow(Direction);
                }

            }
            else
            {
                if (state == FlowerActionState.Blowing)
                {
                    DeactivateField();
                }
            }
        }

    }

    public void ActivateField(Vector3 Direction)
    {
        PushFieldCanvas.transform.GetChild(0).GetComponent<Image>().enabled = true;
        GetComponent<FlowerActionStateManager>().SetActionState(FlowerActionState.Blowing);
        CurrentDirection = Direction;
        TargetDirection = Direction;
    }

    public void DeactivateField()
    {
        CurrentDirection = Vector3.zero;
        PushFieldCanvas.transform.GetChild(0).GetComponent<Image>().enabled = false;
        GetComponent<FlowerActionStateManager>().SetActionState(FlowerActionState.Normal);
    }

    public void Follow(Vector3 Direction)
    {
        TargetDirection = Direction;
        Turn();
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


        List<GameObject> Temp = new List<GameObject>();

        while (Angle < FlowerData.PushFieldAngle)
        {
            RaycastHit[] Allhits = Physics.RaycastAll(transform.position + Vector3.up*CastHeight, v, FlowerData.PushFieldRadius);
            Angle += DetectionAngleInterval;
            v = Quaternion.Euler(0, Angle, 0) * Init;
            for(int i = 0; i < Allhits.Length; i++)
            {
                GameObject obj = Allhits[i].collider.gameObject;
                if (CompareTag("Player"))
                {
                    if (!obj.CompareTag("Player"))
                    {
                        if (!Temp.Contains(obj))
                        {
                            Temp.Add(obj);
                            SetObjectPushFieldInfo(obj);
                            if (!AllObjectsInField.Contains(obj))
                            {
                                AllObjectsInField.Add(obj);
                            }
                        }
                    }
                }
                else
                {
                    if (obj.CompareTag("Player"))
                    {
                        if (!Temp.Contains(obj))
                        {
                            Temp.Add(obj);
                            SetObjectPushFieldInfo(obj);
                            if (!AllObjectsInField.Contains(obj))
                            {
                                AllObjectsInField.Add(obj);
                            }
                        }
                    }
                }
            }
        }

        List<GameObject> Remove = new List<GameObject>();

        for(int i = 0; i < AllObjectsInField.Count; i++)
        {
            if (!Temp.Contains(AllObjectsInField[i]))
            {
                Remove.Add(AllObjectsInField[i]);
            }
        }

        for(int i = 0; i < Remove.Count; i++)
        {
            if (Remove[i].GetComponent<DetectPushField>())
            {
                Remove[i].GetComponent<DetectPushField>().InPushField = false;
            }
            AllObjectsInField.Remove(Remove[i]);
        }
    }

    private void SetObjectPushFieldInfo(GameObject obj)
    {
        if (obj.GetComponent<DetectPushField>())
        {
            obj.GetComponent<DetectPushField>().InPushField = true;
            Vector3 Direction = obj.transform.position - transform.position;
            Direction.y = 0;
            Direction.Normalize();
            obj.GetComponent<DetectPushField>().Direction = Direction;
        }
        
    }
}
