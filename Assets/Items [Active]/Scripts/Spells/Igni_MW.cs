using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Igni Spell", menuName = "Item/Active/Spell/Igni")]
public class Igni_MW : BaseSpell
{
    public override void Use()
    {
        Debug.Log("Used IGNI spell");
    }
}
