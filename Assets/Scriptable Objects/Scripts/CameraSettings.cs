using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Camera Settings", menuName = "Camera Settings")]
public class CameraSettings : ScriptableObject
{
    [Header("Camera Settings")]
    public float mouseSensitivityX;
    public float mouseSensitivityY;
    public float rollSpeed;
    public float rollAmplitude;
    public float overlayCameraLag;
    [Space(10)]

    [Header("Overlay Camera Settings")]
    public float lerpSpeed;
    public float lerpMax;
    public float lerpUpAmplitude;
    public float lerpSideAmplitude;
    public float lerpFwdAmplitude;
}
