using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureBallFly : MonoBehaviour
{
    public float Speed;
    public Vector3 Direction;
    public float MaxDis;

    private float DisTraveled;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Direction * Speed * Time.deltaTime;
        DisTraveled += Speed * Time.deltaTime;
        if (DisTraveled >= MaxDis)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
