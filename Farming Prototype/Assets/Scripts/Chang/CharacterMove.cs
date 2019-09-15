using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    public float NormalSpeed;

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
        var CharacterMoveInfo = GetComponent<CharacterMoveInfo>();
        int Horizontal = 0;
        int Vertical = 0;

        if (InputRight())
        {
            Horizontal++;
        }

        if (InputLeft())
        {
            Horizontal--;
        }

        if (InputUp())
        {
            Vertical++;
        }

        if (InputDown())
        {
            Vertical--;
        }

        if (Horizontal == 0)
        {
            CharacterMoveInfo.Speed = NormalSpeed * Vector2.up * Vertical;
        }
        else if (Vertical == 0)
        {
            CharacterMoveInfo.Speed = NormalSpeed * Vector2.right * Horizontal;
            if (Horizontal > 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
        else
        {
            CharacterMoveInfo.Speed = new Vector2(Horizontal, Vertical) / Mathf.Sqrt(2) * NormalSpeed;
            if (Horizontal > 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }

    private bool InputRight()
    {
        return Input.GetKey(KeyCode.D);
    }

    private bool InputLeft()
    {
        return Input.GetKey(KeyCode.A);
    }

    private bool InputUp()
    {
        return Input.GetKey(KeyCode.W);
    }

    private bool InputDown()
    {
        return Input.GetKey(KeyCode.S);
    }
}
