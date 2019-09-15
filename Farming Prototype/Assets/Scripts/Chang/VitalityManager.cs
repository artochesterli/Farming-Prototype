using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VitalityManager : MonoBehaviour
{
    public float DigCost;
    public float WaterCost;
    public float CaptureCost;

    public float Vitality;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            EventManager.instance.Fire(new VitalityChange(Vitality = 1));
        }
    }
}
