using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public InventoryObject Inventory;

    public Image[] invSpellImages = new Image[4];
    public Image[] invGunsImages = new Image[2];

    void Start()
    {
        StartCoroutine("updateDisplay");
    }

    IEnumerator updateDisplay()
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
    }

}
