using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolText : MonoBehaviour
{
    public Color ActiveColor;
    public Color DeactiveColor;

    private void OnEnable()
    {
        EventManager.instance.AddHandler<SwtichTool>(OnSwtichTool);
        EventManager.instance.AddHandler<SwtichAction>(OnSwitchActon);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnDestroy()
    {
        EventManager.instance.RemoveHandler<SwtichTool>(OnSwtichTool);
        EventManager.instance.RemoveHandler<SwtichAction>(OnSwitchActon);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnSwtichTool(SwtichTool S)
    {
        GetComponent<Text>().text = "Tool: " + S.CurrentTool.name;
    }

    private void OnSwitchActon(SwtichAction S)
    {
        if (S.CurrentType == ActionType.Tool)
        {
            GetComponent<Text>().color = ActiveColor;
        }
        else
        {
            GetComponent<Text>().color = DeactiveColor;
        }
    }
}
