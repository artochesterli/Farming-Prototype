using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraState
{
    Follow
}

public class CameraManager : MonoBehaviour
{
    public GameObject Character;
    public CameraState State;

    public Vector3 Follow_CameraToCharacter;
    public float Follow_CameraSize;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (State == CameraState.Follow)
        {
            FollowCharacter();
        }
    }

    private void FollowCharacter()
    {
        transform.position = Character.transform.position + Follow_CameraToCharacter;
    }
}
