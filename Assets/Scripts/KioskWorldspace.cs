using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KioskWorldspace : MonoBehaviour
{
    public TextMeshPro textMesh;
    public GameObject[] chronologicalScreens;
    [HideInInspector]
    public int screenPointer;
    public CameraController cameraController;

    private void Start()
    {
        screenPointer = 0;
        //textMesh = GetComponent<TextMeshPro>();
        textMesh.text = "hold F\nTo interact...";
    }

    public void ActivateKiosk()
    {
        textMesh.text = "Click outside of\nkiosk to leave";
        chronologicalScreens[screenPointer].SetActive(true);
        screenPointer++;

        cameraController.kioskCameraMovement = true;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void DeactivateKiosk()
    {
        textMesh.text = "hold F\nTo interact...";
        chronologicalScreens[screenPointer - 1].SetActive(false);
        screenPointer = 0;

        cameraController.kioskCameraMovement = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ButtonPressed()
    {
        Debug.Log("Button Pressed!");
    }
}
