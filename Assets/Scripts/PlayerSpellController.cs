using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

// As of now, PlayerSpellController contains some debug functionality for visualizing spellCasts

/* Purpose of PlayerSpellController.cs
 * 
 * PlayerSpellController.cs handles spell activation and their corresponding animations and effects in which PlayerSpellController.cs...
 * 
 * communicates with InventoryObject to know which spells the player has and which corressponding keys activate them
 * communicates with AnimEventsHandler to have spell effects activate at the right of the spell animations
 * communicates with PlayerAnimController to commence spell animations.
 * communicates with ArmComboParser to commence spells animations that occur in quick succession in order to trigger a powerful spell
 * communicates with PlayerGunController to hide and gun when double hand signs are occurring and show them again once they end.
 */

[RequireComponent(typeof(LineRenderer), typeof(InventoryObject), typeof(PlayerAnimController))]
public class PlayerSpellController : MonoBehaviour
{
    // *** Public variables ****
    [Header("External Interactions")]
    [Tooltip("Allows communication to an InventoryObject")]
    public InventoryObject inventory;
    [Tooltip("Obtains keyboard input")]
    public KeyboardController keyboard;
    [Tooltip("Allows communication to the Arm ArmAnimation Events Handler")]
    public AnimEventsHandler animEventsHandler;
    [Tooltip("Allows communication to the Arm Combo Parser used for chained spell casts")]
    public ArmComboParser armComboParser;
    public PlayerGunController playerGunController;
    [Space(10)]

    // **** Private interactions ****
    private PlayerAnimController animController = null;

    // Initializes just a few components...
    void Start()
    {
        animController = GetComponent<PlayerAnimController>();
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
                TriggerSpell(i);
            }
        }
    }

    // Casts spell at in index for 'inventory.spells' arrary and does not casts nullSpells
    public void TriggerSpell(int index)
    {
        var spell = inventory.spells[index];

        if(spell != inventory.nullSpell)
        {
            if (spell.isOneHanded)
            {
                if(animController.SetParameter((int)ArmAnimParams.SpellAnimIndex, (int)spell.animIndex, (int)ArmAnimLayer.leftArm, (int)ArmAnimState.isIdle)) { 
                    animController.Trigger((int)ArmAnimParams.L_Cast, (int)ArmAnimLayer.leftArm, (int)ArmAnimState.isAny); // Trigger
                    animEventsHandler.SetSpell(spell);
                }
            } else
            {
                if(animController.Trigger((int)ArmAnimParams.LR_Cast, (int)ArmAnimLayer.allLayers, (int)ArmAnimState.isIdle)) { 
                    int[] temp = Array.ConvertAll(spell.animDoubleIndexes, value => (int)value);
                    animController.AssignNew(temp, spell.animDoubleIndexes.Length);
                    animEventsHandler.SetSpell(spell);
                    //playerGunController.HideCurrentGun();
                }
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
