using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAbility : MonoBehaviour
{
    private float GenerateTimeCount;
    private GameObject Field;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        Generating();
    }

    private void CheckInput()
    {
        if (InputAvailable() && CompareTag("Player") && GetComponent<SlimeActionStateManager>().CurrentState == SlimeActionState.Normal)
        {
            GenerateStickyField();
        }
    }

    private void Generating()
    {
        var SlimeData = GetComponent<SlimeMonsterData>();
        if(GetComponent<SlimeActionStateManager>().CurrentState == SlimeActionState.Generating)
        {
            GenerateTimeCount += Time.deltaTime;
            Field.transform.localScale = Vector3.Lerp(new Vector3(SlimeData.FieldInitSize, 1, SlimeData.FieldInitSize), new Vector3(SlimeData.FieldMaxSize, 1, SlimeData.FieldMaxSize), GenerateTimeCount / SlimeData.FieldGenerationTime);
            if (GenerateTimeCount >= SlimeData.FieldGenerationTime)
            {
                GenerateTimeCount = 0;
                Field = null;

                GetComponent<SlimeActionStateManager>().SetActionState(SlimeActionState.Normal);
            }
        }
    }

    private bool InputAvailable()
    {
        return Input.GetMouseButtonDown(0);
    }

    public void GenerateStickyField()
    {
        var SlimeData = GetComponent<SlimeMonsterData>();
        Field = (GameObject)Instantiate(Resources.Load("Chang/Prefabs/Static/StickyField"), transform.position, Quaternion.Euler(0, 0, 0));
        Field.transform.localScale = new Vector3(SlimeData.FieldInitSize, 1, SlimeData.FieldInitSize);

        GetComponent<SlimeActionStateManager>().SetActionState(SlimeActionState.Generating);
    }
}
