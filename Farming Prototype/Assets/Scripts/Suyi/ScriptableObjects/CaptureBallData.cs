using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CaptureBallData", menuName = "FarmingPrototype/CaptureUtil/CaptureBallData", order = 1)]
public class CaptureBallData : CaptureUtilScriptableObject
{
    public float ThrowBallPower = 3f;
    public float ThrowBallToDestinationDuration = 1f;

}
