using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteOrderManager : MonoBehaviour
{
    public float YPivot;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        SetSortingLayerOrder();
    }

    private void SetSortingLayerOrder()
    {
        GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(-(transform.position.y + YPivot - Camera.main.transform.position.y) * 100);
    }
}
