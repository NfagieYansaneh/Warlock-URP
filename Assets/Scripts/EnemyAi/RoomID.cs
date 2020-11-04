using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/* Purpose of RoomID.cs
 * 
 * RoomID.cs holds important data for each room such as:
 * 
 * how many cover positions we have for our Ai entities
 * the navigation mesh area mask so we can all Ai entites to avoid a room when calculating a path from A to B
 * the genericEnemyHandler array which basically definies how many Ai objects can be stored in a room and a means to communicated to those Ai objects in this room
 * the Room ID's for adjacent rooms if we plan to have Ai entities to flank a room
 * 
 * RoomID also contains the width and length of the room which is utilised in AiOverseer in order to spawn or move enemies
 */

public enum Rooms { Lobby, Opening, Alleyway };
public enum NavMeshAreaMask { LobbyMask=3, OpeningMask, AlleywayMask }

public class RoomID : MonoBehaviour
{
    public Rooms room; // We must define what 'room' is within Unity's Inspector
    public NavMeshAreaMask navMeshAreaMask;

    public Transform[]  halfCoverTransforms; // half cover is less desirable than full cover since half cover only provides partial protection for Ai Objects
    public bool[]       halfCoverAvailable;

    public Transform[]  fullCoverTransforms;
    public bool[]       fullCoverAvailable;

    public GenericEnemyHandler[] genericEnemyHandlers = new GenericEnemyHandler[10]; // array that stores the generic enemy handlers for all Ai entities within this room 
    public RoomID[] adjacentRooms;
    public bool full = false; // states whether the room is full. Determined when genericEnemyHandlers[] is full of genericEnemyHandlers.

    public Vector3 topLeft; // birds eye view of the top left part of the room
    //public Vector3 bottomRight; // not really used in anything as of now
    public BoxCollider boxCollider;
    public float width; // room width
    public float length; // room length

    private void OnEnable()
    {
        // initalizes all half & full cover transforms as available 
        halfCoverAvailable = new bool[halfCoverTransforms.Length];
        for (int x = 0; x < halfCoverAvailable.Length; x++) halfCoverAvailable[x] = true;
        fullCoverAvailable = new bool[fullCoverTransforms.Length];
        for (int x = 0; x < fullCoverAvailable.Length; x++) fullCoverAvailable[x] = true;
    }

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();

        // calculates room's navMeshAreaMask which derives from 'room' by merely just adding 3 because the first 3 navigation mesh area
        // masks consist of "Walkable", "Not Walkable", and "Jump"
        navMeshAreaMask = (NavMeshAreaMask)((int)room + 3); 
        //genericEnemyHandlers = new GenericEnemyHandler[10];

        // calculates top left corner of the room
        topLeft = new Vector3(transform.position.x + (boxCollider.bounds.size.x / 2), 0f, transform.position.z - (boxCollider.bounds.size.z/2));
        //bottomRight = new Vector3(transform.position.x - (boxCollider.bounds.size.x / 2), 0f, transform.position.z + (boxCollider.bounds.size.z / 2));

        width = boxCollider.size.x;
        length = boxCollider.size.z;
    }
}
