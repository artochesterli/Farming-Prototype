using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    public float Speed;

    public Vector3 MoveVector;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
    }

    private void CheckInput()
    {
        MoveVector = Vector3.zero;

        bool HaveInput = false;

        if (InputRight())
        {
            HaveInput = true;
            MoveVector += Vector3.right;
        }

        if (InputLeft())
        {
            HaveInput = true;
            MoveVector += Vector3.left;
        }

        if (InputForward())
        {
            HaveInput = true;
            MoveVector += Vector3.forward;
        }

        if (InputBack())
        {
            HaveInput = true;
            MoveVector += Vector3.back;
        }


        if (HaveInput)
        {
            transform.eulerAngles = Vector3.up * Vector3.SignedAngle(Vector3.right, MoveVector, Vector3.up);
        }

        GetComponent<SpeedManager>().SelfSpeedDirection = MoveVector.normalized;

    }

    private bool InputRight()
    {
        return Input.GetKey(KeyCode.D);
    }

    private bool InputLeft()
    {
        return Input.GetKey(KeyCode.A);
    }

    private bool InputForward()
    {
        return Input.GetKey(KeyCode.W);
    }

    private bool InputBack()
    {
        return Input.GetKey(KeyCode.S);
    }
}
