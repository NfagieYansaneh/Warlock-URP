using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class UiManager : MonoBehaviour
{
    public InventoryObject Inventory;

    public Image[] invSpellImages = new Image[4];
    public Image[] invGunsImages = new Image[2];
    public Text[] invGunsTexts = new Text[3];

    void Start()
    {
        Inventory.uiManager = GetComponent<UiManager>();

        for (int i = 0; i < Inventory.spells.Length; i++)
        {
            invSpellImages[i].color = Inventory.spells[i].color;
        }

        for (int i = 0; i < Inventory.guns.Length; i++)
        {
            invGunsImages[i].color = Inventory.guns[i].color;
        }
    }

    public void updateGunDisplay()
    {
        BaseGun[] guns = Inventory.guns;

        invGunsImages[0].color = guns[Inventory.curGunIndex].color;
        invGunsImages[1].color = guns[1 - Inventory.curGunIndex].color;

        invGunsTexts[0].text = Convert.ToString(guns[Inventory.curGunIndex].ammoInMag);
        invGunsTexts[1].text = Convert.ToString("/ " + guns[Inventory.curGunIndex].ammo);
        invGunsTexts[2].text = Convert.ToString(guns[1 - Inventory.curGunIndex].ammo);
    }

    public void updateSpellDisplay(int x)
    {
        invSpellImages[x].color = Inventory.spells[x].color;
    }

    /* IEnumerator updateDisplay()
     {
         while (true)
         {
             for (int i = 0; i < Inventory.spells.Length; i++)
             {
                 invSpellImages[i].color = Inventory.spells[i].color;
             }

             for (int i = 0; i < Inventory.guns.Length; i++)
             {
                 invGunsImages[i].color = Inventory.guns[i].color;
             }

             yield return new WaitForSeconds(0.05f);
         }
     } */

}
