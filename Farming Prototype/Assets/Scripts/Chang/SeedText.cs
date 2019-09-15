using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeedText : MonoBehaviour
{
    public Color ActiveColor;
    public Color DeactiveColor;

    private void OnEnable()
    {
        EventManager.instance.AddHandler<SetSeedNumber>(OnSetSeedNum);
        EventManager.instance.AddHandler<SwtichAction>(OnSwitchActon);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnDestroy()
    {
        EventManager.instance.RemoveHandler<SetSeedNumber>(OnSetSeedNum);
        EventManager.instance.RemoveHandler<SwtichAction>(OnSwitchActon);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnSetSeedNum(SetSeedNumber S)
    {
        GetComponent<Text>().text = "Seed: " + S.Num.ToString();
    }

    private void OnSwitchActon(SwtichAction S)
    {
        if (S.CurrentType == ActionType.Seed)
        {
            GetComponent<Text>().color = ActiveColor;
        }
        else
        {
            GetComponent<Text>().color = DeactiveColor;
        }
    }
}
