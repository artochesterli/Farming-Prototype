using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreatField : MonoBehaviour
{
    public GameObject FieldStandOn;
    public float FieldAreaSize;

    private int layermask;
    // Start is called before the first frame update
    void Start()
    {
        layermask = 1 << LayerMask.NameToLayer("Field");
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        CheckFieldStandOn();
    }



    private void CheckFieldStandOn()
    {
        var CharacterMoveInfo = GetComponent<CharacterMoveInfo>();
        Vector2 OriPoint = (Vector2)transform.position + CharacterMoveInfo.PivotPoint;
        float ColliderWidth = CharacterMoveInfo.ColliderWidth;
        float ColliderHeight = CharacterMoveInfo.ColliderHeight;

        RaycastHit2D hit = Physics2D.Raycast(OriPoint, Vector2.down, ColliderHeight / 2, layermask);
        if (hit)
        {
            FieldStandOn = hit.collider.gameObject;
        }
        else
        {
            FieldStandOn = null;
        }
    }

    private void CheckInput()
    {
        var CharacterMoveInfo = GetComponent<CharacterMoveInfo>();
        var ItemHoldingInfo = GetComponent<ItemHoldingInfo>();

        if (InputAvailable())
        {

            System.Type type = ItemHoldingInfo.ToolList[ItemHoldingInfo.ToolHoldingIndex].GetType();

            if (type == typeof(Shovel))
            {
                if (FieldStandOn == null)
                {
                    float X = transform.position.x+ CharacterMoveInfo.PivotPoint.x;
                    if (X > 0)
                    {
                        X += FieldAreaSize / 2;
                    }
                    else
                    {
                        X -= FieldAreaSize / 2;
                    }

                    float Y = transform.position.y+CharacterMoveInfo.PivotPoint.y - CharacterMoveInfo.ColliderHeight/2;
                    if (Y > 0)
                    {
                        Y += FieldAreaSize / 2;
                    }
                    else
                    {
                        Y -= FieldAreaSize / 2;
                    }

                    int XCount = (int)(X / FieldAreaSize);
                    int YCount = (int)(Y / FieldAreaSize);
                    

                    Instantiate(Resources.Load("Chang/Prefabs/Field"), new Vector3(XCount * FieldAreaSize, YCount * FieldAreaSize, 0), Quaternion.Euler(0, 0, 0));
                }
            }
            else if (type == typeof(WaterKettle))
            {

            }
        }
    }

    private bool InputAvailable()
    {
        return Input.GetMouseButtonDown(0);
    }
}
