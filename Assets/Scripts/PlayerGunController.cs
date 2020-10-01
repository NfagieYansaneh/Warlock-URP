using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

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
    [Tooltip("Obtains camera position for firing guns (their raycasts)")]
    public Transform cameraTransform;
    [Tooltip("For communicating with, or updating, Ui")]
    public UiManager uiManager;

    // **** Private variables ****
    private GameObject[] pooledObjects = new GameObject[2];
    private GameObject objectToPool = null;
    private PlayerAnimController AnimController = null;

    private void Start()
    {
        AnimController = GetComponent<PlayerAnimController>();
        //StartCoroutine("DebugShooting");
    }

    /*WaitForSeconds debugTime = new WaitForSeconds(0.05f);
    IEnumerator DebugShooting()
    {
        while (true) { 
            inventory.guns[inventory.curGunIndex].FireRaycast();
            yield return debugTime;
        }
    }*/

    // Handles mouse keys
    private void Update()
    {

        // switches gun based on scroll wheel
        if (keyboard.Keys.mouseScroll != 0f)
        {
            int prevIndex = inventory.curGunIndex;
            int curIndex = 1 - inventory.curGunIndex;
            if (inventory.guns[curIndex] != inventory.nullGun)
            {
                if(AnimController.SetParameter((int)AnimParams.GunAnimIndex, (int)inventory.guns[curIndex].animIndex, (int)AnimLayer.rightArm, (int)AnimState.isIdle))
                {
                    AnimController.Trigger((int)AnimParams.MouseScroll, (int)AnimLayer.rightArm, (int)AnimState.isIdle);
                    inventory.curGunIndex = 1 - inventory.curGunIndex;
                    UpdateDisplayedGuns(inventory.curGunIndex);
                }
                AnimController.Trigger((int)AnimParams.MouseScroll, (int)AnimLayer.rightArm, (int)AnimState.isIdle);
            } else {
                if (inventory.guns[prevIndex] != inventory.nullGun)
                {
                    if(AnimController.SetParameter((int)AnimParams.GunAnimIndex, -1, (int)AnimLayer.rightArm, (int)AnimState.isIdle)) { 
                        AnimController.Trigger((int)AnimParams.MouseScroll, (int)AnimLayer.rightArm, (int)AnimState.isIdle);
                        inventory.curGunIndex = 1 - inventory.curGunIndex;
                        UpdateDisplayedGuns(inventory.curGunIndex);
                    }
                }
                
            }

            // Optimize later
        }

        // This is for bullet travel for player's guns
        for (int index = 0; index < inventory.guns[inventory.curGunIndex].pooledBullets.Length; index++)
        {

            if (inventory.guns[inventory.curGunIndex].pooledBullets[index].activeSelf)
            {
                if(Vector3.Distance(cameraTransform.position, inventory.guns[inventory.curGunIndex].pooledBullets[index].transform.position) >=
                    Vector3.Distance(cameraTransform.position, inventory.guns[inventory.curGunIndex].pooledTrailEndPositions[index]))
                {
                    inventory.guns[inventory.curGunIndex].pooledTrailRenderers[index].emitting = false;
                    inventory.guns[inventory.curGunIndex].pooledBullets[index].SetActive(false);
                    continue;
                }

                inventory.guns[inventory.curGunIndex].pooledBullets[index].transform.position += inventory.guns[inventory.curGunIndex].pooledBullets[index].transform.forward * 1.5f;
            }
        }

        // fires gun if we left click
        if (keyboard.Keys.mouse1)
        {
            if (inventory.guns[inventory.curGunIndex] != inventory.nullGun && inventory.guns[inventory.curGunIndex].ammoInMag > 0)
            {
                if (AnimController.SetParameter((int)AnimParams.GunActionAnimIndex, (int)glock18c_GW.gunActionAnimations.glock18c_Fire, (int)AnimLayer.rightArm, (int)AnimState.isIdle))
                {
                    AnimController.Trigger((int)AnimParams.GunTriggerAnim, (int)AnimLayer.rightArm, (int)AnimState.isIdle);
                    inventory.guns[inventory.curGunIndex].FireAnim();
                }
            }
        }

        if (keyboard.Keys.keyR)
        {
            if(inventory.guns[inventory.curGunIndex] != inventory.nullGun)
            {
                Debug.Log("R");
                if (AnimController.SetParameter((int)AnimParams.GunActionAnimIndex, (int)glock18c_GW.gunActionAnimations.glock18c_Reload, (int)AnimLayer.allLayers, (int)AnimState.isIdle))
                {
                    AnimController.Trigger((int)AnimParams.GunTriggerAnim, (int)AnimLayer.allLayers, (int)AnimState.isIdle);
                    inventory.guns[inventory.curGunIndex].Reload((int)glock18c_GW.gunActionAnimations.glock18c_Reload);
                }
            }
        }

        // ADS if we right click
        if (keyboard.Keys.mouse2)
        {
            if (inventory.guns[inventory.curGunIndex] != inventory.nullGun)
            {
                if (AnimController.SetParameter((int)AnimParams.GunActionAnimIndex, (int)glock18c_GW.gunActionAnimations.glock18c_ThreeFire, (int)AnimLayer.rightArm, (int)AnimState.isIdle))
                {
                    AnimController.Trigger((int)AnimParams.GunTriggerAnim, (int)AnimLayer.rightArm, (int)AnimState.isIdle);
                    inventory.guns[inventory.curGunIndex].ADS();
                }
            }
        }

        // secondary fires gun if we middle mouse
        if (keyboard.Keys.mouse3)
        {
            CastAlternate(inventory.curGunIndex);
        }
    }
    
    // for adding guns
    public void UpdateGuns(int index)
    {
        if (inventory.guns[index] != inventory.nullGun)
        {
            //Start playing equip animation
            AnimController.SetParameter((int)AnimParams.GunAnimIndex, (int)inventory.guns[index].animIndex, (int)AnimLayer.rightArm, (int)AnimState.isIdle);
            AnimController.Trigger((int)AnimParams.MouseScroll, (int)AnimLayer.rightArm, (int)AnimState.isIdle);

            objectToPool = Instantiate(inventory.guns[index].gunModel, gunPlacemant.position,
                gunPlacemant.transform.rotation, gunPlacemant);

            pooledObjects[index] = objectToPool;
            inventory.guns[index].Created(objectToPool, cameraTransform);
            UpdateDisplayedGuns(index);
        }
    } 

    // for switching guns
    public void UpdateDisplayedGuns(int index)
    {
        for(int i=0; i<pooledObjects.Length; i++)
        {
            if (inventory.guns[i] != inventory.nullGun)
            {
                if (i != index)
                {
                    pooledObjects[i].SetActive(false);
                    for(int x=0; x<inventory.guns[i].pooledBullets.Length; x++)
                    {
                        inventory.guns[i].pooledBullets[x].SetActive(false);
                        inventory.guns[i].pooledTrailRenderers[x].emitting = false;
                    }
                    
                }
                else pooledObjects[i].SetActive(true);
            }
        }

        uiManager.UpdateGunDisplay();
    }

    public void CastGun(int index)
    {
        inventory.guns[index].FireAnim();
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
