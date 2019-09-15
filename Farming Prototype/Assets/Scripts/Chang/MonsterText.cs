using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterText : MonoBehaviour
{
    public Color ActiveColor;
    public Color DeactiveColor;

    private void OnEnable()
    {
        EventManager.instance.AddHandler<SetMonsterNum>(OnSetMonsterNum);
        EventManager.instance.AddHandler<SwtichAction>(OnSwitchActon);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnDestroy()
    {
        EventManager.instance.RemoveHandler<SetMonsterNum>(OnSetMonsterNum);
        EventManager.instance.RemoveHandler<SwtichAction>(OnSwitchActon);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnSetMonsterNum(SetMonsterNum S)
    {
        GetComponent<Text>().text = "Water Element: " + S.Num.ToString();
    }

    private void OnSwitchActon(SwtichAction S)
    {
        if (S.CurrentType == ActionType.Place)
        {
            GetComponent<Text>().color = ActiveColor;
        }
        else
        {
            GetComponent<Text>().color = DeactiveColor;
        }
    }
}
