using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Purpose of CameraSettings.cs
 * 
 * CameraSettings.cs stores basic settings for the camera such as the sensitivity of the camera, the speed at which the arm's should sway around
 * whilst we move, etc. 
 * 
 * All of these settings are mainly utilised by the CameraController.cs and OverlayCameraController.cs
 *
 */

[CreateAssetMenu(fileName = "New Camera Settings", menuName = "Camera Settings")]
public class CameraSettings : ScriptableObject
{
    [Header("Camera Settings")]
    public float mouseSensitivityX;
    public float mouseSensitivityY;
    public float rollSpeed; // speed at which the camera tilts in response to player movement
    public float rollAmplitude; // maximum amount the camera can tilt
    //public float overlayCameraLag;
    public float defaultFOV;
    [Space(10)]

    [Header("Overlay Camera Settings")]
    public float lerpSpeed; // speed at which the arm's should sway
    public float lerpMax; // maximum distance the arms can sway away from natural position
    public float lerpUpAmplitude; // defines how much the arm's should sway upwards or...
    public float lerpSideAmplitude; // sideways or...
    public float lerpFwdAmplitude; // forwards

    [Header("Kiosk Camera Settings")]
    public float kioskSensitivity; // a scalar that is meant to decrease the mouse sensitivity when interacting with kiosk so it's easier to interact with
    public float desiredKioskFOV; // desired field of view of the camera when interacting with a kiosk
}
