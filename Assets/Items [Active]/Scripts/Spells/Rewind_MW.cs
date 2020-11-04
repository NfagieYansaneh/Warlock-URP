using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Purpose of Rewind_MW.cs
 * 
 * Rewind_MW.cs contains "MW" in its name which means Magic Weapon
 * 
 * Rewind_MW.cs is merely another spell the player can use. But as for now, its merely logs into the debug
 * terminal that is has been casted.
 */

[CreateAssetMenu(fileName = "New Rewind Spell", menuName = "Item/Active/Spell/Rewind")]
public class Rewind_MW : BaseSpell
{
    
    public override void Use()
    {
        Debug.Log("Activated REWIND Spell");
    }
}
