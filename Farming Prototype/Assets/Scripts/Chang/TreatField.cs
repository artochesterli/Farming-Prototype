using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreatField : MonoBehaviour
{
    public GameObject FieldStandOn;
    public bool OpenPos;
    public float FieldAreaSize;
    public float FootOffset;

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

        RaycastHit2D hit1 = Physics2D.Raycast(OriPoint+Vector2.right*FootOffset, Vector2.down, ColliderHeight / 2, layermask);
        RaycastHit2D hit2 = Physics2D.Raycast(OriPoint+ Vector2.left*FootOffset, Vector2.down, ColliderHeight / 2, layermask);
        if (hit1 && hit2 && hit1.collider.gameObject==hit2.collider.gameObject)
        {
            FieldStandOn = hit1.collider.gameObject;
        }
        else
        {
            FieldStandOn = null;
        }

        if (!hit1 && !hit2)
        {
            OpenPos = true;
        }
        else
        {
            OpenPos = false;
        }
    }

    private void CheckInput()
    {
        var CharacterMoveInfo = GetComponent<CharacterMoveInfo>();
        var ItemHoldingInfo = GetComponent<ItemHoldingInfo>();

        if (InputAvailable())
        {
            ActionType type = GetComponent<ActionSelection>().CurrentType;
            var VitalityManager = GetComponent<VitalityManager>();


            if (type == ActionType.Tool)
            {
                System.Type ToolType = ItemHoldingInfo.ToolList[ItemHoldingInfo.ToolHoldingIndex].GetType();

                if (ToolType == typeof(Shovel))
                {
                    if (OpenPos)
                    {
                        float X = transform.position.x + CharacterMoveInfo.PivotPoint.x;
                        if (X > 0)
                        {
                            X += FieldAreaSize / 2;
                        }
                        else
                        {
                            X -= FieldAreaSize / 2;
                        }

                        float Y = transform.position.y + CharacterMoveInfo.PivotPoint.y - CharacterMoveInfo.ColliderHeight / 2;
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
                        EventManager.instance.Fire(new VitalityChange(VitalityManager.Vitality -= VitalityManager.DigCost));
                    }
                }
                else if (ToolType == typeof(WaterKettle))
                {
                    if (FieldStandOn)
                    {
                        FieldStandOn.GetComponent<Field>().State = FieldState.Wet;
                        EventManager.instance.Fire(new VitalityChange(VitalityManager.Vitality -= VitalityManager.WaterCost));
                    }
                }
            }
            else if(type == ActionType.Seed)
            {
                if (FieldStandOn && !FieldStandOn.GetComponent<Field>().Seed)
                {
                    var ItemHoldInfo = GetComponent<ItemHoldingInfo>();
                    if (ItemHoldInfo.SeedNumber > 0)
                    {
                        ItemHoldingInfo.SeedNumber--;
                        EventManager.instance.Fire(new SetSeedNumber(ItemHoldInfo.SeedNumber));
                        FieldStandOn.GetComponent<Field>().Seed = (GameObject)Instantiate(Resources.Load("Chang/Prefabs/Seed"), FieldStandOn.transform.position, Quaternion.Euler(0, 0, 0));
                    }
                }
            }
            else if(type == ActionType.Place)
            {
                if (FieldStandOn)
                {
                    var ItemHoldInfo = GetComponent<ItemHoldingInfo>();
                    if (ItemHoldingInfo.MonsterCaptured[0].number > 0)
                    {
                        ItemHoldingInfo.MonsterCaptured[0].number--;
                        EventManager.instance.Fire(new SetMonsterNum(ItemHoldInfo.MonsterCaptured[0].number));
                        GameObject Monster = (GameObject)Instantiate(Resources.Load("Chang/Prefabs/WaterElement"), FieldStandOn.transform.position, Quaternion.Euler(0, 0, 0));
                        Monster.GetComponent<MonsterStateInfo>().State = MonsterState.Captured;
                        Monster.GetComponent<MonsterStateInfo>().SetAttributes();
                    }
                }
            }
        }
    }

    private bool InputAvailable()
    {
        return Input.GetMouseButtonDown(0);
    }
}
