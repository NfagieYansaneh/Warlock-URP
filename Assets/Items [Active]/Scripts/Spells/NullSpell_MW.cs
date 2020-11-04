using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Purpose of NullSpell_MW.cs
 * 
 * NullGun_MW.cs contains "GW" in its name which means Magic Weapon 
 * 
 * NullGun_MW.cs is our "NULL" when it comes to spells. Therefore, if we only have three spells in our player's inventory
 * then that last empty slot in our player's gun inventory will be actually a NullSpell_MW spot
 * 
 */

[CreateAssetMenu(fileName = "New Null Spell", menuName = "Item/Active/Spell/Null")]
public class NullSpell_MW : BaseSpell
{
    public override void Use()
    {
        Debug.Log("Used NULL spell");
    }
}
