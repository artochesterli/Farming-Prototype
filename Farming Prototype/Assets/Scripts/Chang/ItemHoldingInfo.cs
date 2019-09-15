using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBundle
{
    GameObject Monster;
    int Number;
    public MonsterBundle(GameObject monster, int num)
    {
        Monster = monster;
        Number = num;
    }
}

public class ItemHoldingInfo : MonoBehaviour
{
    public List<Tool> ToolList;
    public int ToolHoldingIndex;
    public int SeedNumber;
    public List<MonsterBundle> MonsterCaptured;

    // Start is called before the first frame update
    void Start()
    {
        ToolList = new List<Tool>();
        ToolList.Add(new Shovel());
        ToolList.Add(new WaterKettle());
        ToolList.Add(new CaptureNet());

        SeedNumber = 3;

        MonsterCaptured = new List<MonsterBundle>();

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SwtichTools()
    {

    }
}
