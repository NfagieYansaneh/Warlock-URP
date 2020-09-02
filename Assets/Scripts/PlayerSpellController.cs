using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

// As of now, PlayerSpellController contains some debug functionality for visualizing spellCasts

[RequireComponent(typeof(LineRenderer), typeof(InventoryObject), typeof(PlayerAnimController))]
public class PlayerSpellController : MonoBehaviour
{
    // *** Public variables ****
    [Header("External Interactions")]
    [Tooltip("Allows communication to an InventoryObject")]
    public InventoryObject inventory;
    [Tooltip("Obtains keyboard input")]
    public KeyboardController keyboard;
    [Tooltip("Allows communication to the Arm Animation Events Handler")]
    public ArmAnimEventsHandler armAnimEventsHandler;
    [Tooltip("Allows communication to the Arm Combo Parser used for chained spell casts")]
    public ArmComboParser armComboParser;
    [Space(10)]

    // **** Private interactions ****
    private PlayerAnimController AnimController = null;

    // Initializes just a few components...
    void Start()
    {
        AnimController = GetComponent<PlayerAnimController>();
    }

    // Checks key inputs specifically for spell casting
    private void Update()
    {
        // Handles Alpha keys (1 - 4) to activates respective spells
        for (int i = 0; i < keyboard.Keys.spellKeys.Length; i++)
        {
            // Some spells display an active crosshair (active as in dynamic and interacts w/ the world space)
            CastActiveCrosshair(i);

            if (keyboard.Keys.spellKeys[i])
            {
                // Activates spell
                CastSpell(i);
            }
        }
    }

    // Casts spell at in index for 'inventory.spells' arrary and does not casts nullSpells
    public void CastSpell(int index)
    {
        var spell = inventory.spells[index];

        if(spell != inventory.nullSpell)
        {
            if (spell.isOneHanded)
            {
                AnimController.SetParameter((int)AnimParams.CastingIndex, (int)spell.animationIndex, (int)AnimLayer.leftArm, (int)AnimState.isIdle);
                AnimController.Trigger((int)AnimParams.L_Cast, (int)AnimLayer.leftArm, (int)AnimState.isIdle); // Trigger
                armAnimEventsHandler.SetSpell(spell);
            } else
            {
                int[] temp = Array.ConvertAll(spell.animationDoubleIndex, value => (int)value);
                AnimController.AssignNew(temp, spell.animationDoubleIndex.Length);
                AnimController.Trigger((int)AnimParams.LR_Cast, (int)AnimLayer.allLayers, (int)AnimState.isIdle);
                armAnimEventsHandler.SetSpell(spell);
            }
        }
    }

    // Casts active crosshair (a dynamic crosshair that typically interacts w/ the environment) at index for
    // 'inventory.spells' and does not cast active crosshairs for nullSpells
    public void CastActiveCrosshair(int index)
    {
        if (inventory.spells[index] != inventory.nullSpell)
        {
            inventory.spells[index].AlternativeCrosshairActive();
        }
    }
}
