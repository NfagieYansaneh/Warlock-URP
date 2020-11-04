using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Purpose of GunPickup.cs
 * 
 * GunPickup.cs stores a BaseGun object whilst setting its gameObject to a "GunPickup" tag
 * Thus, when the player colliders with GunPickup's collider and identifies that it is a gun pickup, via its tag
 * the player acquires the gun for GunPickup's gun slot and then promptly destroys/disables GunPickup's gameObject
 * 
 */

public class GunPickup : MonoBehaviour
{
    public BaseGun gunSlot;

    private void Start()
    {
        gameObject.tag = "GunPickup";
    }
}
