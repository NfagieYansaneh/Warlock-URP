using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Purpose of SpellPickup.cs
 * 
 * Spellickup.cs stores a BaseSpell object whilst setting its gameObject to a "SpellPickup" tag
 * Thus, when the player colliders with SpellPickup's collider and identifies that it is a spell pickup, via its tag
 * the player acquires the gun for SpellPickup's gun slot and then promptly destroys/disables SpellPickup's gameObject
 * 
 */

public class SpellPickup : MonoBehaviour
{
    public BaseSpell spellSlot;

    private void Start()
    {
        gameObject.tag = "SpellPickup";
    }
}
