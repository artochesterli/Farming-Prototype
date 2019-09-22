using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectStickyField : MonoBehaviour
{
    public bool InStickyField;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("StickyField"))
        {
            InStickyField = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("StickyField"))
        {
            InStickyField = false;
        }
    }


}
