using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VitalityBar : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        EventManager.instance.AddHandler<VitalityChange>(OnVitalityChange);
    }

    private void OnDestroy()
    {
        EventManager.instance.RemoveHandler<VitalityChange>(OnVitalityChange);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnVitalityChange(VitalityChange V)
    {
        GetComponent<Image>().fillAmount = V.CurrentVitality;
    }
}
