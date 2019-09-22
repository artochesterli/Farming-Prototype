using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectDanger : MonoBehaviour
{
    public float AlertDis;
    public float ResponseDis;
    public float SaveDis;

    public float SaveDetectAngle;
    public float InDangerDetectAngle;
    public List<Form> IgnoredForms;

    public GameObject DetectedPlayer;
    public GameObject DetectedBall;

    public float CastHeight;
    public float CastInterval;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool Detect(float RangeAngle, float DetectDis)
    {
        Vector3 Direction = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0) * Vector3.right;

        Direction= Quaternion.Euler(0, -RangeAngle / 2, 0) * Direction;

        Vector3 Init = Direction;

        float Angle = 0;

        bool InDanger = false;

        List<GameObject> DangerObjects = new List<GameObject>();

        while (Angle < RangeAngle)
        {
            Direction = Quaternion.Euler(0, Angle, 0) * Init;
            RaycastHit hit;
            if(Physics.Raycast(transform.position + Vector3.up * CastHeight, Direction, out hit, DetectDis))
            {
                if(hit.collider.CompareTag("Player") && !InIgnoredForms(hit.collider.gameObject.GetComponent<FormManager>().CurrentForm) || hit.collider.CompareTag("CaptureBall"))
                {
                    DangerObjects.Add(hit.collider.gameObject);

                    InDanger = true;
                }
            }

            Angle += CastInterval;
        }

        if (InDanger)
        {
            for(int i = 0; i < DangerObjects.Count; i++)
            {
                if (DangerObjects[i].CompareTag("CaptureBall"))
                {
                    DetectedBall = DangerObjects[i];
                }
                else
                {
                    DetectedPlayer = DangerObjects[i];
                }
            }
        }
        else
        {
            DetectedBall = null;
            DetectedPlayer = null;
        }

        return InDanger;
    }

    private bool InIgnoredForms(Form f)
    {
        for(int i = 0; i < IgnoredForms.Count; i++)
        {
            if (IgnoredForms[i] == f)
            {
                return true;
            }
        }

        return false;
    }
}
