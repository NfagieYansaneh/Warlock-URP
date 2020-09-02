using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Null Spell", menuName = "Item/Active/Spell/Null")]
public class NullSpell_MW : BaseSpell
{
    public override void Use()
    {
        Debug.Log("Used NULL spell");
    }
}
