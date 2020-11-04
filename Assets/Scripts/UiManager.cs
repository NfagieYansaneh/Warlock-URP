using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

/* Purpose of UiManager.cs
 * 
 * UiManager.cs handles the user interface for the player to intuitively display ammo count,
 * spells, health, and score multiplier. UiManager is connected to the inventory object of 
 * the player so when we wish to update the Ui display, UiManager.cs will acquire the player's
 * current inventory data to update corressponding Ui elements such as ammo count
 */

public class UiManager : MonoBehaviour
{
    public InventoryObject Inventory;

    public Image[] invSpellImages = new Image[4];
    public Image[] invGunsImages = new Image[2];

    public Image trueHealthImage;
    public Image greyHealthImage;

    public Image scoreBarImage;
    public Text scoreTierText;

    public Text[] invGunsTexts = new Text[3];

    void Start()
    {
        // Initializes spell and gun Ui elements

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

    // Updates gun display Ui elements
    public void UpdateGunDisplay()
    {
        BaseGun[] guns = Inventory.guns;

        invGunsImages[0].color = guns[Inventory.curGunIndex].color;
        invGunsImages[1].color = guns[1 - Inventory.curGunIndex].color;

        invGunsTexts[0].text = Convert.ToString(guns[Inventory.curGunIndex].ammoInMag);
        invGunsTexts[1].text = Convert.ToString("/ " + guns[Inventory.curGunIndex].maxAmmoInMag);
        invGunsTexts[2].text = Convert.ToString(guns[1 - Inventory.curGunIndex].ammoInMag);
    }

    // Updates a specific spell slot Ui element
    public void UpdateSpellDisplay(int x)
    {
        invSpellImages[x].color = Inventory.spells[x].color;
    }

    // Updates health display Ui elements
    public void UpdateHealthDisplay(float trueHealth, float greyHealth)
    {
        trueHealthImage.rectTransform.localScale = new Vector3(trueHealth / 100f, 1f, 1f);
        greyHealthImage.rectTransform.localScale = new Vector3(greyHealth / 100f, 1f, 1f);
    }

    // Updates score multiplier Ui elements
    public void UpdateScoreDisplay(float scoreAtTier, int scoreTier)
    {
        switch(scoreTier)
        {
            case 0:
                scoreTierText.text = "x0";
                break;

            case 1:
                scoreTierText.text = "x1";
                break;

            case 2:
                scoreTierText.text = "x2";
                break;

            case 3:
                scoreTierText.text = "x3";
                break;
        }

        scoreBarImage.rectTransform.localScale = new Vector3(scoreAtTier / 100f, 1f, 1f);
    }

}
