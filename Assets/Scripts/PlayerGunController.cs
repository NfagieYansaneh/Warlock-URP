using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[RequireComponent(typeof(PlayerAnimController), typeof(InventoryObject))]
public class PlayerGunController : MonoBehaviour
{
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

    private GameObject[] pooledObjects = new GameObject[2]; // consists of the guns in out inventory
    private GameObject objectToPool = null;
    private PlayerAnimController animController = null; // animation controller 

    private void Start()
    {
        animController = GetComponent<PlayerAnimController>();
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

    private void Update()
    {
        // Everyframe, we move the player's active bullets into the direction they were initially travelling
        CheckAndMoveBullets();

        // switches gun based on scroll wheel and handles their equiping animation but only works for single handed weapons as of now
        if (keyboard.Keys.mouseScroll != 0f)
        {
            SwitchGuns();
        }

        // Checks if we tried to reload our gun
        if (keyboard.Keys.keyR)
        {
            // Make cursor move when i am reloading (Maybe?)
            CurrentGunReload();
        }

        // Checks if we tried to fire our gun
        if (keyboard.Keys.mouse1)
        {
            CurrentGunPrimaryFire();
        }

        // Aim down sights if we right click but for debugging sake, it also activates our secondary fire
        if (keyboard.Keys.mouse2)
        {
            CurrentGunSecondaryFire();
            //CurrentGunADS();
        }

        // secondary fires gun if we middle mouse but does not work as of now
        if (keyboard.Keys.mouse3)
        {
            CurrentGunSecondaryFire();
        }
    }
    
    // for adding guns such as when we pick them up and 'index' refers to which inventory position we should be updated with that new picked up gun
    public void UpdateGunInventoryAndInstantiateThem(int index)
    {
        // Start playing equip animation of the new gun or queues it if we are in a animation
        animController.SetParameter((int)ArmAnimParams.GunAnimCatergory, (int)inventory.guns[index].gunAnimCatergory, (int)ArmAnimLayer.rightArm, (int)ArmAnimState.isAny);
        animController.Trigger((int)ArmAnimParams.MouseScroll, (int)ArmAnimLayer.rightArm, (int)ArmAnimState.isAny);

        // instantiates the gun we just got
        objectToPool = Instantiate(inventory.guns[index].gunModel, gunPlacemant.position,
            gunPlacemant.transform.rotation, gunPlacemant);

        // adds it into our pooledObjects
        pooledObjects[index] = objectToPool;
        inventory.guns[index].Created(objectToPool, cameraTransform);
        UpdateDisplayedGunsAndUI(index);
        inventory.curGunIndex = index;
    } 

    // for switching guns
    public void UpdateDisplayedGunsAndUI(int index)
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

    public void CurrentGunPrimaryFire()
    {
        if (inventory.guns[inventory.curGunIndex] != inventory.nullGun)
        {
            inventory.guns[inventory.curGunIndex].PrimaryFire(animController);
        }
    }

    public void CurrentGunSecondaryFire()
    {
        if (inventory.guns[inventory.curGunIndex] == inventory.nullGun)
        {
            animController.Trigger((int)ArmAnimParams.R_Emote, (int)ArmAnimLayer.rightArm, (int)ArmAnimState.isIdle);
            return;
        }

        inventory.guns[inventory.curGunIndex].SecondaryFire(animController);
    }

    public void CurrentGunReload()
    {
        if (inventory.guns[inventory.curGunIndex] != inventory.nullGun && inventory.guns[inventory.curGunIndex].ammoInMag != inventory.guns[inventory.curGunIndex].maxAmmoInMag)
        {
            // Does reload animation only if both arms are idle
            if (animController.SetParameter((int)ArmAnimParams.GunActionAnimIndex, inventory.guns[inventory.curGunIndex].reloadAnimIndex, (int)ArmAnimLayer.allLayers, (int)ArmAnimState.isIdle))
            {
                animController.Trigger((int)ArmAnimParams.L_GunTriggerAnim, (int)ArmAnimLayer.leftArm, (int)ArmAnimState.isAny);
                animController.Trigger((int)ArmAnimParams.R_GunTriggerAnim, (int)ArmAnimLayer.rightArm, (int)ArmAnimState.isAny);

                inventory.guns[inventory.curGunIndex].Reload(); // Triggers the reload animation for the gun
            }
        }
    }

    public void CurrentGunADS()
    {
        inventory.guns[inventory.curGunIndex].ADS();
    }

    public void SwitchGuns()
    {
        int prevIndex = inventory.curGunIndex;
        int nextIndex = 1 - inventory.curGunIndex;

        // checks if the gun we are switching to is not an empty gun (a gun that doesn't exist)
        if (inventory.guns[nextIndex] != inventory.nullGun)
        {
            // Plays the equip animation for a gun if that gun is of a different type we are holding
            if (animController.SetParameter((int)ArmAnimParams.GunAnimCatergory, (int)inventory.guns[nextIndex].gunAnimCatergory, (int)ArmAnimLayer.rightArm, (int)ArmAnimState.isIdle))
            {
                animController.Trigger((int)ArmAnimParams.MouseScroll, (int)ArmAnimLayer.rightArm, (int)ArmAnimState.isAny);
                inventory.curGunIndex = nextIndex;
                UpdateDisplayedGunsAndUI(inventory.curGunIndex);
            }

            // Replays the equip animation for a gun if it is of the same type we are holding
            animController.Trigger((int)ArmAnimParams.MouseScroll, (int)ArmAnimLayer.rightArm, (int)ArmAnimState.isIdle);

        }
        else
        {
            // if we are holding a gun and the next gun we are switching to does not exist, we transition the right arm into its T-pose idle state in which we are now not holding a gun
            if (inventory.guns[inventory.curGunIndex] != inventory.nullGun)
            {
                if (animController.SetParameter((int)ArmAnimParams.GunAnimCatergory, -1, (int)ArmAnimLayer.rightArm, (int)ArmAnimState.isIdle))
                {
                    animController.Trigger((int)ArmAnimParams.MouseScroll, (int)ArmAnimLayer.rightArm, (int)ArmAnimState.isAny);
                    inventory.curGunIndex = nextIndex;
                    UpdateDisplayedGunsAndUI(inventory.curGunIndex);
                }
            }

        }

        // Optimize later
    }

    public void HideCurrentGun()
    {
        pooledObjects[inventory.curGunIndex].SetActive(false);
    }

    public void ShowCurrentGun()
    {
        pooledObjects[inventory.curGunIndex].SetActive(true);
    }

    public void CheckAndMoveBullets()
    {
        for (int index = 0; index < inventory.guns[inventory.curGunIndex].pooledBullets.Length; index++)
        {

            // Collision detection for bullets
            bool bulletCollided = false;

            if (inventory.guns[inventory.curGunIndex].pooledBullets[index].activeSelf)
            {
                Collider[] colliders = Physics.OverlapSphere(inventory.guns[inventory.curGunIndex].pooledBullets[index].transform.position, 0.2f,
                    inventory.guns[inventory.curGunIndex].bulletLayerMask);

                foreach (Collider c in colliders)
                {
                    if (c.CompareTag("Enemy"))
                    {
                        c.gameObject.GetComponent<GenericEnemyHandler>().Hit(inventory.guns[inventory.curGunIndex].damage);
                        inventory.guns[inventory.curGunIndex].OnEnemyImpact();
                    }

                    // promptly disables projectile
                    if (inventory.guns[inventory.curGunIndex].pooledBullets[index].activeSelf)
                    {
                        inventory.guns[inventory.curGunIndex].DisableBulletFromPool(index);
                        bulletCollided = true;
                    }
                }

                if (bulletCollided)
                {
                    continue;
                }

                BaseGun currentGun = inventory.guns[inventory.curGunIndex];
                // Add this to the enemies as well
                float bulletExponentialPercentage = Mathf.InverseLerp(0f, 15f,
                    Vector3.Distance(currentGun.pooledBulletsStartPositions[index], currentGun.pooledBullets[index].transform.position));

                bulletExponentialPercentage = Mathf.Clamp01(bulletExponentialPercentage);

                float bulletSpeed = currentGun.baseBulletSpeed + (currentGun.maxBulletSpeed - currentGun.baseBulletSpeed) * bulletExponentialPercentage;

                currentGun.pooledBullets[index].transform.position += currentGun.pooledBullets[index].transform.forward * bulletSpeed;
            }

            }
        }
}
