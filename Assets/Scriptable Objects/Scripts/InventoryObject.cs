using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Purpose of InventoryObject.cs
 * 
 * InventoryObject.cs handles our inventory by storing our guns and spells in corressponding BaseGun and BaseSpell arrays
 * in which other scripts access in order to access the players inventory. InventoryObject.cs also contains a few functions
 * in order to quickly override inventory items. In order to add an item, we would simply OverrideeAtFirstFind(); of a null inventory item
 * and replace it with our new item
 *
 * InventoryObjects.cs ...
 * 
 * communicates with UiManager to update the Ui display of the player's Inventory when we override inventory items with a new item
 * 
 */


[CreateAssetMenu(fileName = "New Inventory", menuName = "Systems/Inventory")]
public class InventoryObject : ScriptableObject
{
    public BaseSpell[] spells = new BaseSpell[4];
    public BaseGun[] guns = new BaseGun[2];

    // for debugging purposes, we can pre-define our player's inventory before we are in the game
    public BaseSpell[] presetSpells = new BaseSpell[4];
    public BaseGun[] presetGuns = new BaseGun[2];

    // nullSpell & nullGun exist when questioning whether we have no spell or no gun in our current hands
    public BaseSpell nullSpell;
    public BaseGun nullGun;

    //this is for communicating and updating our UI
    public UiManager uiManager;

    // holds the curGunIndex for knowing which gun is being held
    public int curGunIndex = 0;
    
    // clears our spells by forcing spells[] and guns[] to our presetSpells[] and presetGuns[]
    public void OnDestroy()
    {
        ForceToPreset();
    }
    public void OnEnable()
    {
        ForceToPreset();
    }

    // Overrides an inventory item at its first find
    public int OverrideAtFirstFind(BaseSpell newItem, BaseSpell caseItem)
    {
        for(int i = 0; i < spells.Length; i++)
        {
            if (spells[i] == caseItem)
            {
                spells[i] = newItem;
                uiManager.UpdateSpellDisplay(i);
                return i;
            }
        }

        return -1;
    }
    public int OverrideAtFirstFind(BaseGun newItem, BaseGun caseItem)
    {
        for (int i = 0; i < guns.Length; i++)
        {
            if (guns[i] == caseItem)
            {
                guns[i] = newItem;
                uiManager.UpdateGunDisplay();
                return i;
            }
        }

        return -1;
    }

    // Overrides an inventory item at its index
    public void OverrideAtIndex(BaseSpell newItem, int index)
    {
        spells[index] = newItem;
        uiManager.UpdateSpellDisplay(index);
    }
    public void OverrideAtIndex(BaseGun newItem, int index)
    {
        guns[index] = newItem;
        uiManager.UpdateGunDisplay();
    }

    // Simply forces our inventory to our preset inventory (definied before the game begins)
    public void ForceToPreset()
    {
        for (int i = 0; i < spells.Length; i++)
        {
            spells[i] = presetSpells[i];
        }

        for (int i = 0; i < guns.Length; i++)
        {
            guns[i] = presetGuns[i];
        }
    }

}
