using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Rewind Spell", menuName = "Item/Active/Spell/Rewind")]
public class Rewind_MW : BaseSpell
{
    
    public override void Use()
    {
        Debug.Log("Activated REWIND Spell");
    }
}
