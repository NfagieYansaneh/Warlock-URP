using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Systems/Inventory")]
public class InventoryObject : ScriptableObject
{
    public BaseSpell[] spells = new BaseSpell[4];
    public BaseGun[] guns = new BaseGun[2];

    public BaseSpell[] presetSpells = new BaseSpell[4];
    public BaseGun[] presetGuns = new BaseGun[2];

    // nullSpell & nullGun exist when questioning whether we have no spell or no gun in our current hands
    public BaseSpell nullSpell;
    public BaseGun nullGun;

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
                return i;
            }
        }

        return -1;
    }
    public int OverrideAtFirstFind(BaseGun newItem, BaseGun caseItem)
    {
        for (int i = 0; i < spells.Length; i++)
        {
            if (guns[i] == caseItem)
            {
                guns[i] = newItem;
                return i;
            }
        }

        return -1;
    }

    // Overrides an inventory item at its index
    public void OverrideAtIndex(BaseSpell newItem, int index)
    {
        spells[index] = newItem;
    }
    public void OverrideAtIndex(BaseGun newItem, int index)
    {
        guns[index] = newItem;
    }

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
