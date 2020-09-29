using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum Rooms { Lobby, Opening, Alleyway };
public enum NavMeshAreaMask { LobbyMask=3, OpeningMask, AlleywayMask }

public class RoomID : MonoBehaviour
{
    public Rooms room;
    public NavMeshAreaMask navMeshAreaMask;

    public Transform[]  halfCoverTransforms;
    public bool[]       halfCoverAvailable;

    public Transform[]  fullCoverTransforms;
    public bool[]       fullCoverAvailable;

    public GenericEnemyHandler[] genericEnemyHandlers = new GenericEnemyHandler[10];
    public RoomID[] adjacentRooms;
    public bool full = false;

    public Vector3 topLeft;
    public Vector3 bottomRight; // not really used in anything as of now
    public BoxCollider boxCollider;
    public float width;
    public float length;

    // Maybe Optimize Later?

    private void OnEnable()
    {
        halfCoverAvailable = new bool[halfCoverTransforms.Length];
        for (int x = 0; x < halfCoverAvailable.Length; x++) halfCoverAvailable[x] = true;
        fullCoverAvailable = new bool[fullCoverTransforms.Length];
        for (int x = 0; x < fullCoverAvailable.Length; x++) fullCoverAvailable[x] = true;
    }

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        navMeshAreaMask = (NavMeshAreaMask)((int)room + 3);
        //genericEnemyHandlers = new GenericEnemyHandler[10];

        topLeft = new Vector3(transform.position.x + (boxCollider.bounds.size.x / 2), 0f, transform.position.z - (boxCollider.bounds.size.z/2));
        bottomRight = new Vector3(transform.position.x - (boxCollider.bounds.size.x / 2), 0f, transform.position.z + (boxCollider.bounds.size.z / 2));

        width = boxCollider.size.x;
        length = boxCollider.size.z;
    }
}
