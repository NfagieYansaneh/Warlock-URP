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

    private void Start()
    {
        screenPointer = 0;
        //textMesh = GetComponent<TextMeshPro>();
        textMesh.text = "press F\nTo interact...";
    }

    public void ActivateKiosk()
    {
        textMesh.text = "Activated!";
        chronologicalScreens[screenPointer].SetActive(true);
        screenPointer++;
    }

    public void ButtonPressed()
    {
        Debug.Log("Button Pressed!");
    }
}
