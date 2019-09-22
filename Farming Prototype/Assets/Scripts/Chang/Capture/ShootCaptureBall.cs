using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootCaptureBall : MonoBehaviour
{
    private float PreparingTimeCount;
    private Vector3 Direction;
    private GameObject CaptureBall;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        if (CaptureBall)
        {
            PrepareShooting();
        }
    }

    private void CheckInput()
    {
        if (InputAvailable() && StateAvailable())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                Direction = hit.point - hit.point.y * Vector3.up - (transform.position-transform.position.y*Vector3.up);
                Direction.Normalize();
            }

            GenerateCaptureBall();
        }
    }

    private bool InputAvailable()
    {
        return Input.GetMouseButtonDown(0);
    }

    private bool StateAvailable()
    {
        var CharacterActionStateManager = GetComponent<CharacterActionStateManager>();
        return (CharacterActionStateManager.CurrentState == CharacterActionState.Normal || CharacterActionStateManager.CurrentState == CharacterActionState.Charging) &&
            !GetComponent<DetectStickyField>().InStickyField && !GetComponent<DetectPushField>().InPushField;
    }

    private void GenerateCaptureBall()
    {
        var CharacterData = GetComponent<CharacterData>();
        CaptureBall = (GameObject)Instantiate(Resources.Load("Chang/Prefabs/Character/CaptureBall"), transform.position + Direction + CharacterData.CaptureBallHeight * Vector3.up, Quaternion.Euler(0, 0, 0));
        CaptureBall.transform.localScale = Vector3.zero;
        var CaptureBallFly = CaptureBall.GetComponent<CaptureBallFly>();
        CaptureBallFly.Speed = CharacterData.CaptureBallSpeed;
        CaptureBallFly.Direction =  Direction;
        CaptureBallFly.MaxDis = CharacterData.CaptureBallMaximalDis;

        GetComponent<CharacterActionStateManager>().SetActionState(CharacterActionState.ShootPreparing);
    }

    private void PrepareShooting()
    {
        var CharacterData = GetComponent<CharacterData>();
        PreparingTimeCount += Time.deltaTime;
        CaptureBall.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one * CharacterData.CaptureBallSize, PreparingTimeCount / CharacterData.CaptureBallShootPreparingTime);
        if (PreparingTimeCount > CharacterData.CaptureBallShootPreparingTime)
        {
            PreparingTimeCount = 0;
            CaptureBall.GetComponent<CaptureBallFly>().enabled = true;
            CaptureBall = null;
            GetComponent<CharacterActionStateManager>().SetActionState(CharacterActionState.Normal);
        }
    }
}
