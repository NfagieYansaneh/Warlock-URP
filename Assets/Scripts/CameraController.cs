using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // **** Public variables ****
    [Header("External Interactions")]
    [Tooltip("Contains variables that defines: camera speed, camera roll speed, etc.")]
    public CameraSettings settings;
    [Tooltip("Allows the camera to interact with the transform of the parent player in cases where we need to change its rotation")]
    public Transform parentPlayer;
    [Tooltip("Obtains keyboard input")]
    public KeyboardController keyboard;
    public Transform overlayArms;

    // **** Private variables ****
    private float xRotation = 0f;

    //'destRoll' stores the current z-rotation our camera should lerp to
    private float destRoll = 0f;
    // 'curRoll' stores the current z-rotation that our camera is at as it tries to lerp to 'destRoll'
    private float curRoll = 0f;

    private float mouseX = 0f;
    private float mouseY = 0f;

    // **** Private interactions ****
    private Camera mainCamera = null;

    // Initializes just a few components...
    void Start()
    {
        mainCamera = GetComponent<Camera>();
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    void LateUpdate()
    {
        mouseX = keyboard.Keys.mouseX * settings.mouseSensitivityX * Time.deltaTime;
        mouseY = keyboard.Keys.mouseY * settings.mouseSensitivityY * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Clamps the x-rotation of our character so we can only look straight up & down

        // our 'destRoll' is based on the keys we press. Pressing 'A' leans us to the left and pressing 'B' leans us to the right
        destRoll = settings.rollAmplitude * (keyboard.Keys.keyA - keyboard.Keys.keyD); 
        curRoll = Mathf.Lerp(curRoll, destRoll, settings.rollSpeed);

        // Rotates our camera for leaning and looking up & down
        transform.localRotation = Quaternion.Euler(xRotation, 0f, curRoll);
        // Rotates our player so we can turn left or right
        parentPlayer.Rotate(0f, mouseX, 0f);

        Debug.DrawRay(transform.position, transform.forward, Color.red, Time.deltaTime); // Debugging
    }
}
