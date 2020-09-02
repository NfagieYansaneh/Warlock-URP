using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Bloodlust Spell", menuName = "Item/Active/Spell/Bloodlust")]
public class Bloodlust_MW : BaseSpell
{

    public override void Use()
    {
        Debug.Log("Used BLOODLUST spell");
    }
}
