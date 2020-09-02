using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellPickup : MonoBehaviour
{
    public BaseSpell spellSlot;

    private void Start()
    {
        gameObject.tag = "SpellPickup";
    }
}
