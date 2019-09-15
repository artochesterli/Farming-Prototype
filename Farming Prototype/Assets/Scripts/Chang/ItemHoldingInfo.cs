using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemHoldingInfo : MonoBehaviour
{
    public List<Tool> ToolList;
    public int ToolHoldingIndex;
    public int SeedNumber;
    public List<Monster> MonsterCaptured;

    // Start is called before the first frame update
    void Start()
    {
        ToolList = new List<Tool>();
        ToolList.Add(new Shovel());
        ToolList.Add(new WaterKettle());
        ToolList.Add(new CaptureNet());

        MonsterCaptured = new List<Monster>();
        MonsterCaptured.Add(new WaterElement());

        EventManager.instance.Fire(new SwtichAction(ActionType.Tool));
        EventManager.instance.Fire(new SwtichTool(ToolList[ToolHoldingIndex]));
        EventManager.instance.Fire(new SetSeedNumber(SeedNumber));
        EventManager.instance.Fire(new SetMonsterNum(0));
        
    }

    // Update is called once per frame
    void Update()
    {
        SwtichItems();
    }

    private void SwtichItems()
    {
        if (InputAvailable())
        {
            ActionType type = GetComponent<ActionSelection>().CurrentType;
            if (type == ActionType.Tool)
            {
                ToolHoldingIndex = (ToolHoldingIndex + 1) % ToolList.Count;
                EventManager.instance.Fire(new SwtichTool(ToolList[ToolHoldingIndex]));
            }
        }
    }

    private bool InputAvailable()
    {
        return Input.GetKeyDown(KeyCode.Tab);
    }

    
}
