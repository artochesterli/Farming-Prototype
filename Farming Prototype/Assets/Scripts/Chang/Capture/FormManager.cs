using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Form
{
    Character,
    Slime,
    Dryad
}

public class FormManager : MonoBehaviour
{
    public Form CurrentForm;

    private List<Component> TemporaryMonsterComponents;
    // Start is called before the first frame update
    void Start()
    {
        TemporaryMonsterComponents = new List<Component>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
    }

    private void CheckInput()
    {
        if (InputSlime())
        {
            if (CurrentForm == Form.Slime)
            {
                BackToCharacter();
            }
            else
            {
                SwtichToSlime();
            }
        }

        if (InputDryad())
        {
            if (CurrentForm == Form.Dryad)
            {
                BackToCharacter();
            }
            else
            {
                SwtichToDryad();
            }
        }
    }



    private bool InputSlime()
    {
        return Input.GetKeyDown(KeyCode.Alpha1);
    }

    private bool InputDryad()
    {
        return Input.GetKeyDown(KeyCode.Alpha2);
    }

    private void BackToCharacter()
    {
        CurrentForm = Form.Character;

        ClearTempComponents();
        ActivateCharacter();

        GetComponent<MeshFilter>().mesh = Resources.Load("Chang/Models/Character", typeof(Mesh)) as Mesh;
        GetComponent<Renderer>().material = Resources.Load("Chang/Material/Character", typeof(Material)) as Material;


        var CharacterData = GetComponent<CharacterData>();
        EventManager.instance.Fire(new CallSetCharacterSpeed(CharacterData.NormalSpeed, CharacterData.StickySlowDownSpeed));
    }

    private void SwtichToSlime()
    {
        CurrentForm = Form.Slime;

        ClearTempComponents();

        gameObject.AddComponent<SlimeMonsterData>();
        gameObject.AddComponent<SlimeAbility>();
        gameObject.AddComponent<SlimeActionStateManager>();
        TemporaryMonsterComponents.Add(GetComponent<SlimeMonsterData>());
        TemporaryMonsterComponents.Add(GetComponent<SlimeAbility>());
        TemporaryMonsterComponents.Add(GetComponent<SlimeActionStateManager>());

        GetComponent<MeshFilter>().mesh = Resources.Load("Chang/Models/Slime", typeof(Mesh)) as Mesh;
        GetComponent<Renderer>().material = Resources.Load("Chang/Material/SlimeBody", typeof(Material)) as Material;

        DeactivateCharacter();


        var SlimeData = GetComponent<SlimeMonsterData>();
        EventManager.instance.Fire(new CallSetCharacterSpeed(SlimeData.NormalSpeed, SlimeData.StickySlowDownSpeed));
    }

    private void SwtichToDryad()
    {
        CurrentForm = Form.Dryad;

        ClearTempComponents();

        gameObject.AddComponent<DryadData>();
        gameObject.AddComponent<DryadAbility>();
        gameObject.AddComponent<DryadActionStateManager>();
        TemporaryMonsterComponents.Add(GetComponent<DryadData>());
        TemporaryMonsterComponents.Add(GetComponent<DryadAbility>());
        TemporaryMonsterComponents.Add(GetComponent<DryadActionStateManager>());

        GetComponent<MeshFilter>().mesh = Resources.Load("Chang/Models/Dryad", typeof(Mesh)) as Mesh;
        GetComponent<Renderer>().material = Resources.Load("Chang/Material/Dryad", typeof(Material)) as Material;

        DeactivateCharacter();

        var DryadData = GetComponent<DryadData>();
        EventManager.instance.Fire(new CallSetCharacterSpeed(DryadData.NormalSpeed, DryadData.StickySlowDownSpeed));
    }

    private void ActivateCharacter()
    {
        GetComponent<CharacterActionStateManager>().enabled = true;
        GetComponent<ShootCaptureBall>().enabled = true;
    }

    private void DeactivateCharacter()
    {
        GetComponent<CharacterActionStateManager>().enabled = false;
        GetComponent<ShootCaptureBall>().enabled = false;
    }

    private void ClearTempComponents()
    {
        for(int i = 0; i < TemporaryMonsterComponents.Count; i++)
        {
            Destroy(TemporaryMonsterComponents[i]);
        }
        TemporaryMonsterComponents.Clear();
    }
}
