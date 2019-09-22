using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "FarmingPrototype/PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    public float MovingSpeed;
    public float RotationSpeed;
    public float CaptureChargeSpeed = 2f;
    public float CaptureDischargeInterval = 1f;
    public float CaptureDischargeSpeed = 5f;
    public float ThrowBallRange = 3f;
    public float ThrowBallPower = 3f;
    public float ThrowBallToDestinationDuration = 0.5f;
    public float ThrowBallInterval = 0.2f;
    public GameObject ThrowBallPrefab;
    public LayerMask ThrowBallMouseCastLayer;

}
