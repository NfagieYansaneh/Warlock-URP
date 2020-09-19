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

    // Maybe Optimize Later?

    private void OnEnable()
    {
        halfCoverAvailable = new bool[halfCoverTransforms.Length];
        for (int x = 0; x < halfCoverAvailable.Length; x++) halfCoverAvailable[x] = true;
        fullCoverAvailable = new bool[fullCoverTransforms.Length];
        for (int x = 0; x < fullCoverAvailable.Length; x++) fullCoverAvailable[x] = true;
    }
}
