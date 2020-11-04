using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Purpose of BloodLust_MW.cs
 * 
 * BloodLust_MW.cs contains "MW" in its name which means Magic Weapon
 * 
 * BloodLust_MW.cs is merely another spell the player can use. But as for now, its merely logs into the debug
 * terminal that is has been casted.
 */

[CreateAssetMenu(fileName = "New Bloodlust Spell", menuName = "Item/Active/Spell/Bloodlust")]
public class Bloodlust_MW : BaseSpell
{

    public override void Use()
    {
        Debug.Log("Used BLOODLUST spell");
    }
}
