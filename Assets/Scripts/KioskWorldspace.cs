using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/* Purpose of KioskWorldspace.cs
 * 
 * KioskWorldspace.cs handles the functionality for the kiosk in the scene. A kisosk is a public area used for providing information with an interactive display
 * The purpose of my kiosk, as of now, is to spawn enemies by interacting with it. In my final version, the kiosk will be used for buying guns and spells
 * 
 * KioskWorldspace.cs ...
 * communicates with CameraController such that when we are interacting with our kiosk, our mouseSensitivty decreases so our camera turns in response of our mouse movement at a slower rate
 * communicates with AiOverseer to spawn and interact with Ai elemnents in the scene for debugging purposes
 */

public class KioskWorldspace : MonoBehaviour
{
    public TextMeshPro textMesh;

    // contains an array of screen displays which is indexed by screenPointer in order to switch screen displaying different types of information
    // it is called chronologicalScreens because the screens in chronologicalScreens is odered in the order they are meant to be displayed
    public GameObject[] chronologicalScreens; 
    [HideInInspector]
    public int screenPointer;
    public CameraController cameraController;

    public AiOverseer aiOverseer;
    private void Start()
    {
        screenPointer = 0;
        //textMesh = GetComponent<TextMeshPro>();
        textMesh.text = "hold F\nTo interact..."; // Basic, idle display text
    }

    public void ActivateKiosk()
    {
        textMesh.text = "Click outside of\nkiosk to leave";
        chronologicalScreens[screenPointer].SetActive(true);
        screenPointer++; // ready to activate the next screen

        // Allows the mouse to now properly interact with the Kiosk
        cameraController.kioskCameraMovement = true;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void DeactivateKiosk()
    {
        textMesh.text = "hold F\nTo interact...";
        chronologicalScreens[screenPointer - 1].SetActive(false); // disables active screen
        screenPointer--;

        // Returns mouse back to normal state
        cameraController.kioskCameraMovement = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Communicates with Ai Overseer to spawn an Ai entity into the lobby (Made for the sake of debugging)
    // Our Kiosk will be placed in the same room as lobby so we just say spawn ai "InRoom"
    public void OverseerSpawnAiInRoom()
    {
        aiOverseer.SpawnRandomlyInRoom(Rooms.Lobby);
        //Debug.Log("Button Pressed!");
    }

    // Communciates with Ai Overseer to kill all Ai entities in a givien room. Enum "Rooms" helps identify said room
    public void OverseerKillAllInRoom(int roomIndex)
    {
        aiOverseer.KillAllInRoom((Rooms)roomIndex);
    }

    // Communicates with Ai Overseer to move all Ai entities from one room ("lobby" in this instance) to another room ("opening" in this instance)
    public void OverseerMoveAllToOpening()
    {
        aiOverseer.MoveAllAiToRoom(Rooms.Lobby, Rooms.Opening);
    }

    // Communicates with Ai Overseer to move Ai entities to 'Flank' a room by moving Ai entities to one of the adjacent rooms for the targeted room
    // we intend to 'Flank'. Therefore, instead of causing Ai entities to take the shortest path in order to get into a room, we can instruct them to move
    // towards another room first in order to take a more obscure and 'Flanking' path towards the player.
    public void FlankRoomThroughRoomFromRoom()
    {
        aiOverseer.FlankRoomThroughRoom(Rooms.Opening, Rooms.Alleyway, Rooms.Lobby);
    }
}
