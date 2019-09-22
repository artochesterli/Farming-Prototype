using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Form
{
    Character,
    Slime,
    Dryad,
    Bull,
    Flower
}

public class FormManager : MonoBehaviour
{
    public Form CurrentForm;

    private List<Component> TemporaryMonsterComponents;
    private Collider TemporaryCollider;
    // Start is called before the first frame update
    void Start()
    {
        TemporaryMonsterComponents = new List<Component>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        if (GetComponent<MeshCollider>())
        {
            GetComponent<MeshCollider>().convex = true;
        }
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

        if (InputBull())
        {
            if(CurrentForm == Form.Bull)
            {
                BackToCharacter();
            }
            else
            {
                SwtichToBull();
            }
        }

        if (InputFlower())
        {
            if (CurrentForm == Form.Flower)
            {
                BackToCharacter();
            }
            else
            {
                SwtichToFlower();
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

    private bool InputBull()
    {
        return Input.GetKeyDown(KeyCode.Alpha3);
    }

    private bool InputFlower()
    {
        return Input.GetKeyDown(KeyCode.Alpha4);
    }

    private void BackToCharacter()
    {
        CurrentForm = Form.Character;

        ClearTempComponents();
        ClearTemporaryCollider();
        ActivateCharacter();

        GetComponent<MeshFilter>().mesh = Resources.Load("Chang/Models/Character", typeof(Mesh)) as Mesh;
        GetComponent<Renderer>().material = Resources.Load("Chang/Material/Character", typeof(Material)) as Material;


        var CharacterData = GetComponent<CharacterData>();
        GetComponent<SpeedManager>().SetSpeedData(CharacterData.NormalSpeed, CharacterData.StickySlowDownSpeed, CharacterData.PushSpeed);

    }

    private void SwtichToSlime()
    {
        CurrentForm = Form.Slime;

        ClearTempComponents();
        ClearTemporaryCollider();

        gameObject.AddComponent<SlimeMonsterData>();
        gameObject.AddComponent<SlimeAbility>();
        gameObject.AddComponent<SlimeActionStateManager>();
        TemporaryMonsterComponents.Add(GetComponent<SlimeMonsterData>());
        TemporaryMonsterComponents.Add(GetComponent<SlimeAbility>());
        TemporaryMonsterComponents.Add(GetComponent<SlimeActionStateManager>());

        GetComponent<MeshFilter>().mesh = Resources.Load("Chang/Models/Slime", typeof(Mesh)) as Mesh;
        GetComponent<Renderer>().material = Resources.Load("Chang/Material/SlimeBody", typeof(Material)) as Material;

        DeactivateCharacter();

        GameObject temp = (GameObject)Instantiate(Resources.Load("Chang/Prefabs/Creatures/Slime"));

        Mesh mesh = temp.GetComponent<MeshCollider>().sharedMesh;
        gameObject.AddComponent<MeshCollider>();
        GetComponent<MeshCollider>().sharedMesh = mesh;
        GetComponent<MeshCollider>().convex = true;
        TemporaryCollider = GetComponent<MeshCollider>();

        Destroy(temp);

        var SlimeData = GetComponent<SlimeMonsterData>();
        GetComponent<SpeedManager>().SetSpeedData(SlimeData.NormalSpeed, SlimeData.StickySlowDownSpeed, SlimeData.PushSpeed);
    }

    private void SwtichToDryad()
    {
        CurrentForm = Form.Dryad;

        ClearTempComponents();
        ClearTemporaryCollider();

        gameObject.AddComponent<DryadData>();
        gameObject.AddComponent<DryadAbility>();
        gameObject.AddComponent<DryadActionStateManager>();
        TemporaryMonsterComponents.Add(GetComponent<DryadData>());
        TemporaryMonsterComponents.Add(GetComponent<DryadAbility>());
        TemporaryMonsterComponents.Add(GetComponent<DryadActionStateManager>());

        GetComponent<MeshFilter>().mesh = Resources.Load("Chang/Models/Dryad", typeof(Mesh)) as Mesh;
        GetComponent<Renderer>().material = Resources.Load("Chang/Material/Dryad", typeof(Material)) as Material;

        DeactivateCharacter();
        GetComponent<BoxCollider>().enabled = true;

        var DryadData = GetComponent<DryadData>();
        GetComponent<SpeedManager>().SetSpeedData(DryadData.NormalSpeed, DryadData.StickySlowDownSpeed, DryadData.PushSpeed);
    }

    private void SwtichToBull()
    {
        CurrentForm = Form.Bull;

        ClearTempComponents();
        ClearTemporaryCollider();

        gameObject.AddComponent<BullData>();
        gameObject.AddComponent<BullAbility>();
        gameObject.AddComponent<BullActionStateManager>();
        TemporaryMonsterComponents.Add(GetComponent<BullData>());
        TemporaryMonsterComponents.Add(GetComponent<BullAbility>());
        TemporaryMonsterComponents.Add(GetComponent<BullActionStateManager>());

        GetComponent<MeshFilter>().mesh = Resources.Load("Chang/Models/Bull", typeof(Mesh)) as Mesh;
        GetComponent<Renderer>().material = Resources.Load("Chang/Material/Bull", typeof(Material)) as Material;

        DeactivateCharacter();

        GameObject temp = (GameObject)Instantiate(Resources.Load("Chang/Prefabs/Creatures/Bull"));

        Mesh mesh = temp.GetComponent<MeshCollider>().sharedMesh;
        gameObject.AddComponent<MeshCollider>();
        GetComponent<MeshCollider>().sharedMesh = mesh;
        GetComponent<MeshCollider>().convex = true;
        TemporaryCollider = GetComponent<MeshCollider>();

        Destroy(temp);

        var BullData = GetComponent<BullData>();
        GetComponent<SpeedManager>().SetSpeedData(BullData.NormalSpeed, BullData.StickySlowDownSpeed, BullData.PushSpeed);
    }

    private void SwtichToFlower()
    {
        CurrentForm = Form.Flower;

        ClearTempComponents();
        ClearTemporaryCollider();

        gameObject.AddComponent<FlowerData>();
        gameObject.AddComponent<FlowerAbility>();
        gameObject.AddComponent<FlowerActionStateManager>();
        TemporaryMonsterComponents.Add(GetComponent<FlowerData>());
        TemporaryMonsterComponents.Add(GetComponent<FlowerAbility>());
        TemporaryMonsterComponents.Add(GetComponent<FlowerActionStateManager>());

        GetComponent<MeshFilter>().mesh = Resources.Load("Chang/Models/Flower", typeof(Mesh)) as Mesh;
        GetComponent<Renderer>().material = Resources.Load("Chang/Material/WaterBlue", typeof(Material)) as Material;

        DeactivateCharacter();

        GameObject temp = (GameObject)Instantiate(Resources.Load("Chang/Prefabs/Creatures/Flower"));

        Mesh mesh = temp.GetComponent<MeshCollider>().sharedMesh;
        gameObject.AddComponent<MeshCollider>();
        GetComponent<MeshCollider>().sharedMesh = mesh;
        GetComponent<MeshCollider>().convex = true;
        TemporaryCollider = GetComponent<MeshCollider>();

        Destroy(temp);

        GetComponent<FlowerAbility>().PushFieldCanvas = (GameObject)Instantiate(Resources.Load("Chang/Prefabs/Static/PushFieldCanvas"));
        GetComponent<FlowerAbility>().PushFieldCanvas.transform.position = transform.position;
        GetComponent<FlowerAbility>().PushFieldCanvas.transform.parent = transform;
        GetComponent<FlowerAbility>().PushFieldCanvas.GetComponent<RectTransform>().localRotation= Quaternion.Euler(90, 0, 0);
        GetComponent<FlowerAbility>().PushFieldCanvas.GetComponent<Canvas>().worldCamera = Camera.main;

        var FlowerData = GetComponent<FlowerData>();
        GetComponent<SpeedManager>().SetSpeedData(FlowerData.NormalSpeed, FlowerData.StickySlowDownSpeed, FlowerData.PushSpeed);
    }

    private void ActivateCharacter()
    {
        GetComponent<CharacterActionStateManager>().enabled = true;
        GetComponent<ShootCaptureBall>().enabled = true;
        GetComponent<BoxCollider>().enabled = true;
    }

    private void DeactivateCharacter()
    {
        GetComponent<CharacterActionStateManager>().enabled = false;
        GetComponent<ShootCaptureBall>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;
    }

    private void ClearTempComponents()
    {
        for(int i = 0; i < TemporaryMonsterComponents.Count; i++)
        {
            Destroy(TemporaryMonsterComponents[i]);
        }
        TemporaryMonsterComponents.Clear();
    }

    private void ClearTemporaryCollider()
    {
        Destroy(TemporaryCollider);
        TemporaryCollider = null;
    }
}
