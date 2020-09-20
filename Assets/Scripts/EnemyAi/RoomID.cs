using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum Rooms { Lobby };
public class RoomID : MonoBehaviour
{
    public Rooms room;
    public Transform[] halfCoverTransforms;
    [HideInInspector]
    public bool[] halfCoverAvailable;
    public Transform[] fullCoverTransforms;
    [HideInInspector]
    public bool[] fullCoverAvailable;

    BoxCollider boxCollider;
    [HideInInspector]
    public float[] xyColliderCorners;
    [HideInInspector]
    public float width; // x
    [HideInInspector]
    public float length; // y

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
        //collider.bounds.contains(playerTransform.position)
        xyColliderCorners = new float[2];

        boxCollider = GetComponent<BoxCollider>();
        float a = (boxCollider.size.x / 2);
        float b = (boxCollider.size.y / 2);

        width = boxCollider.size.x;
        length = boxCollider.size.y;

        xyColliderCorners[0] = boxCollider.center.x - a; // x

        xyColliderCorners[1] = boxCollider.center.y - b; // y
    }
}
