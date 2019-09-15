using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterState
{
    Wild,
    Captured
}

public class MonsterStateInfo : MonoBehaviour
{
    public MonsterState State;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetAttributes()
    {
        if (State == MonsterState.Wild)
        {
            GetComponent<BoxCollider2D>().enabled = true;
        }
        else if (State == MonsterState.Captured)
        {
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
