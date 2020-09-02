using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(PlayerAnimController), typeof(InventoryObject))]
public class PlayerGunController : MonoBehaviour
{
    // *** Public variables ****
    [Header("External Interactions")]
    [Tooltip("Allows communication to an InventoryObject")]
    public InventoryObject inventory;
    [Tooltip("Obtains keyboard input")]
    public KeyboardController keyboard;
    [Tooltip("Obtains camera rotation for properly display guns")]
    public Transform gunPlacemant;

    // **** Private variables ****
    private GameObject[] pooledObjects = new GameObject[2];
    private GameObject objectToPool = null;
    private PlayerAnimController AnimController = null;

    private void Start()
    {
        AnimController = GetComponent<PlayerAnimController>();
    }

    // Handles mouse keys
    private void Update()
    {
        // switches gun based on scroll wheel
        if (keyboard.Keys.mouseScroll != 0f)
        {
            inventory.curGunIndex = 1 - inventory.curGunIndex;
            UpdateDisplayedGuns(inventory.curGunIndex);
            ///Debug.Log(inventory.curGunIndex);
        }

        // fires gun if we left click
        if (keyboard.Keys.mouse1)
        {
            CastGun(inventory.curGunIndex);
        }

        // ADS if we right click
        if (keyboard.Keys.mouse2)
        {
            CastADS(inventory.curGunIndex);
        }

        // secondary fires gun if we middle mouse
        if (keyboard.Keys.mouse3)
        {
            CastAlternate(inventory.curGunIndex);
        }
    }
    
    public void UpdateGuns(int index)
    {
        if (inventory.guns[index] != inventory.nullGun)
        {
            objectToPool = Instantiate(inventory.guns[index].gunModel, gunPlacemant.position,
                gunPlacemant.transform.rotation * Quaternion.Euler(inventory.guns[index].rotationOffset), gunPlacemant);

            pooledObjects[index] = objectToPool;
            UpdateDisplayedGuns(index);
        }
    }

    public void UpdateDisplayedGuns(int index)
    {
        for(int i=0; i<pooledObjects.Length; i++)
        {
            if (inventory.guns[i] != inventory.nullGun)
            {
                if (i != index) pooledObjects[i].SetActive(false);
                else pooledObjects[i].SetActive(true);
            }
        }
    }

    public void CastGun(int index)
    {
        inventory.guns[index].Fire();
    }
    public void CastADS(int index)
    {
        inventory.guns[index].ADS();
    }
    public void CastAlternate(int index)
    {
        var gun = inventory.guns[index];

        // Emote test
        if (gun == inventory.nullGun)
        {
            AnimController.Trigger((int)AnimParams.R_Emote, (int)AnimLayer.rightArm, (int)AnimState.isIdle);
            return;
        }

        gun.SecondaryFire();
    }
}
