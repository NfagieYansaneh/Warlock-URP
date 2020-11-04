using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Purpose of glock18c_GW.cs
 * 
 * glock18c_GW.cs contains "GW" in its name which means Gun Weapon
 * 
 * glock18c_GW.cs is the basic template for the glock18c handgun in which it overrides functions defined in BaseGun and
 * provides functionality to functions such as PrimaryFire(); and SecondaryFire(); for example whilst also creating a set of
 * Action animations so we can index glock18c's unique gun firing animations and such
 */

[CreateAssetMenu(fileName = "New glock18c", menuName = "Item/Active/Gun/glock18c")]
public class glock18c_GW : BaseGun
{
    public enum GunActionAnims { Glock18c_Reload, Glock18c_Fire, Glock18c_TwoFire, Glock18c_ThreeFire }; // enum used to index glock18c's unique gun animations
    public enum GunAnimParams { GunActionAnimIndex, GunTriggerAnim }; // enum used to inuitively interact with glock18c's animator's paramters in order to transition from one animation to the next

    public enum GunMods { SingleShot, TwoShotBurst, ThreeShotBurst };
    public GunMods activeGunMod; // enum used to define which active gun modification this gun has 

    public Animator animator;

    private void OnEnable()
    {
        animator = gunModel.GetComponent<Animator>();
    }

    // paramHashes allows us to actually interact with the parameters of our animators, via an ID method.
    private readonly static int[] paramHashes =
    {
        Animator.StringToHash("GunActionAnimIndex"),
        Animator.StringToHash("GunTriggerAnim")
    };

    // PlayAnim take in a value that indexes towards an action animation for glock18c's and sets the correct parameters in order to play it
    public override void PlayAnim(int animation)
    {
        animator.SetInteger(paramHashes[(int)GunAnimParams.GunActionAnimIndex], (int)animation);
        animator.SetTrigger(paramHashes[(int)GunAnimParams.GunTriggerAnim]);
        Debug.Log("Fired glock18c : " + this.name);
    }

    // FireBulletFromPool enables a bullet, sets the correct bullet position and rotation, and activates it
    public override void FireBulletFromPool()
    {
        // raycast is shot forwards from the mainCamera (player's main camera) position to deduce what is directly in front of the player
        Vector3 end = mainCameraTransform.position + (mainCameraTransform.forward * 15f);
        Ray ray = new Ray(mainCameraTransform.position, mainCameraTransform.forward);
        RaycastHit hit;

        // if our raycast collides with anything account for my layer mask (bulletLayerMask)...
        if (Physics.Raycast(ray, out hit, 200f, bulletLayerMask)) // Optimize with a layer mask & make distance short as needed
        {
            // then set our bullet end position to that collision point
            pooledBulletsEndPositions[pooledBulletsIndex] = hit.point;
        }
        else
        {
            // else, set our bullet position to 15 units in front of us
            pooledBulletsEndPositions[pooledBulletsIndex] = end;
        }


        // sets the bullet position at the end of the barrel of the gun as well as updating our pooledBulletsStartPositions array
        pooledBullets[pooledBulletsIndex].transform.position = realGunModel.transform.position +
            (realGunModel.transform.forward * barrelTransformOffset.z) + (realGunModel.transform.up * barrelTransformOffset.y) + (realGunModel.transform.right * barrelTransformOffset.x);
        
        pooledBulletsStartPositions[pooledBulletsIndex] = pooledBullets[pooledBulletsIndex].transform.position;
        pooledBullets[pooledBulletsIndex].transform.LookAt(pooledBulletsEndPositions[pooledBulletsIndex]);

        /*
        if (Physics.Raycast(ray, 5f)) // For when objects are close, our bullets 
            pooledBullets[pooledBulletsIndex].transform.LookAt(end);
        else
        {
            pooledBullets[pooledBulletsIndex].transform.LookAt(pooledBulletsEndPositions[pooledBulletsIndex]);
        }*/

        // enables bullets as well as turning on it's trail renderer
        pooledBullets[pooledBulletsIndex].SetActive(true);
        pooledTrailRenderers[pooledBulletsIndex].emitting = true;

        // basically increments pooledBulletsIndex in a certain manner to loop through our pooledBullets
        if (pooledBulletsIndex == pooledBullets.Length - 1) pooledBulletsIndex = 0;
        else pooledBulletsIndex++;
    }

    // disables a specfic moving bullet in our pool of bullets
    public override void DisableBulletFromPool(int index)
    {
        pooledTrailRenderers[index].emitting = false;
        pooledBullets[index].SetActive(false);
    }

    // PrimaryFire(); burst fires our gun depending on what activeGunMod we have
    public override void PrimaryFire(PlayerAnimController playerAnimController)
    {
        if (ammoInMag > 0)
            switch (activeGunMod) // checks which active gun modification we have
            {
                case GunMods.SingleShot:
                    
                    // Plays the gun and arm animation for a single shot if the right arm is in a idle or interruptable state
                    if (playerAnimController.SetParameter((int)ArmAnimParams.GunActionAnimIndex, (int)GunActionAnims.Glock18c_Fire, (int)ArmAnimLayer.rightArm,
                        (int)ArmAnimState.isIdle, (int)ArmAnimState.isInterrupt))
                    {
                        playerAnimController.Trigger((int)ArmAnimParams.R_GunTriggerAnim, (int)ArmAnimLayer.rightArm, (int)ArmAnimState.isAny);
                        PlayAnim((int)GunActionAnims.Glock18c_Fire);
                    }
                    break;

                case GunMods.TwoShotBurst:

                    // Plays the gun and arm animation for a double shot burst if the right arm is in a idle or interruptable state
                    if (ammoInMag >= 2)
                    {
                        if (playerAnimController.SetParameter((int)ArmAnimParams.GunActionAnimIndex, (int)GunActionAnims.Glock18c_TwoFire, (int)ArmAnimLayer.rightArm,
                            (int)ArmAnimState.isIdle, (int)ArmAnimState.isInterrupt))
                        {
                            playerAnimController.Trigger((int)ArmAnimParams.R_GunTriggerAnim, (int)ArmAnimLayer.rightArm, (int)ArmAnimState.isAny);
                            PlayAnim((int)GunActionAnims.Glock18c_TwoFire);
                        }
                    }

                    // else, play the gun and arm animation for a single shot if the right arm is in a idle or interruptable state
                    else if (ammoInMag == 1)
                    {
                        if (playerAnimController.SetParameter((int)ArmAnimParams.GunActionAnimIndex, (int)GunActionAnims.Glock18c_Fire, (int)ArmAnimLayer.rightArm,
                            (int)ArmAnimState.isIdle, (int)ArmAnimState.isInterrupt))
                        {
                            playerAnimController.Trigger((int)ArmAnimParams.R_GunTriggerAnim, (int)ArmAnimLayer.rightArm, (int)ArmAnimState.isAny);
                            PlayAnim((int)GunActionAnims.Glock18c_Fire);
                        }
                    }
                    break;

                case GunMods.ThreeShotBurst:

                    // Plays the gun and arm animation for a triple shot burst if the right arm is in a idle or interruptable state
                    if (ammoInMag >= 3)
                    {
                        if (playerAnimController.SetParameter((int)ArmAnimParams.GunActionAnimIndex, (int)GunActionAnims.Glock18c_ThreeFire, (int)ArmAnimLayer.rightArm,
                            (int)ArmAnimState.isIdle, (int)ArmAnimState.isInterrupt))
                        {
                            playerAnimController.Trigger((int)ArmAnimParams.R_GunTriggerAnim, (int)ArmAnimLayer.rightArm, (int)ArmAnimState.isAny);
                            PlayAnim((int)GunActionAnims.Glock18c_ThreeFire);
                        }
                    }

                    // else, plays the gun and arm animation for a second shot burst if the right arm is in a idle or interruptable state
                    else if (ammoInMag == 2)
                    {
                        if (playerAnimController.SetParameter((int)ArmAnimParams.GunActionAnimIndex, (int)GunActionAnims.Glock18c_TwoFire, (int)ArmAnimLayer.rightArm,
                            (int)ArmAnimState.isIdle, (int)ArmAnimState.isInterrupt))
                        {
                            playerAnimController.Trigger((int)ArmAnimParams.R_GunTriggerAnim, (int)ArmAnimLayer.rightArm, (int)ArmAnimState.isAny);
                            PlayAnim((int)GunActionAnims.Glock18c_TwoFire);
                        }
                    }

                    // else then play the gun and arm animation for a single shot if the right arm is in a idle or interruptable state
                    else if (ammoInMag == 1)
                    {
                        if (playerAnimController.SetParameter((int)ArmAnimParams.GunActionAnimIndex, (int)GunActionAnims.Glock18c_Fire, (int)ArmAnimLayer.rightArm,
                            (int)ArmAnimState.isIdle, (int)ArmAnimState.isInterrupt))
                        {
                            playerAnimController.Trigger((int)ArmAnimParams.R_GunTriggerAnim, (int)ArmAnimLayer.rightArm, (int)ArmAnimState.isAny);
                            PlayAnim((int)GunActionAnims.Glock18c_Fire);
                        }
                    }
                    break;
            }
    }

    // SecondaryFire(); produces a triple burst shot regardless (unless we don't have enough ammo) of or activeGunMod just for the sake of debugging
    public override void SecondaryFire(PlayerAnimController playerAnimController)
    {
        // Below is just for debugging sake
        // also, in release, this gun is to have no secondary fire

        // Plays the gun and arm animation for a triple shot burst if the right arm is in a idle or interruptable state
        if (ammoInMag >= 3)
        {
            if (playerAnimController.SetParameter((int)ArmAnimParams.GunActionAnimIndex, (int)GunActionAnims.Glock18c_ThreeFire, (int)ArmAnimLayer.rightArm,
                (int)ArmAnimState.isIdle, (int)ArmAnimState.isInterrupt))
            {
                playerAnimController.Trigger((int)ArmAnimParams.R_GunTriggerAnim, (int)ArmAnimLayer.rightArm, (int)ArmAnimState.isAny);
                PlayAnim((int)GunActionAnims.Glock18c_ThreeFire);
            }
        }

        // else, plays the gun and arm animation for a second shot burst if the right arm is in a idle or interruptable state
        else if (ammoInMag == 2)
        {
            if (playerAnimController.SetParameter((int)ArmAnimParams.GunActionAnimIndex, (int)GunActionAnims.Glock18c_TwoFire, (int)ArmAnimLayer.rightArm,
                (int)ArmAnimState.isIdle, (int)ArmAnimState.isInterrupt))
            {
                playerAnimController.Trigger((int)ArmAnimParams.R_GunTriggerAnim, (int)ArmAnimLayer.rightArm, (int)ArmAnimState.isAny);
                PlayAnim((int)GunActionAnims.Glock18c_TwoFire);
            }
        }

        // else then play the gun and arm animation for a single shot if the right arm is in a idle or interruptable state
        else if (ammoInMag == 1)
        {
            if (playerAnimController.SetParameter((int)ArmAnimParams.GunActionAnimIndex, (int)GunActionAnims.Glock18c_Fire, (int)ArmAnimLayer.rightArm,
                (int)ArmAnimState.isIdle, (int)ArmAnimState.isInterrupt))
            {
                playerAnimController.Trigger((int)ArmAnimParams.R_GunTriggerAnim, (int)ArmAnimLayer.rightArm, (int)ArmAnimState.isAny);
                PlayAnim((int)GunActionAnims.Glock18c_Fire);
            }
        }
    }

    // As for now, Aim Down Sights does nothing
    public override void ADS()
    {
       // animator.SetInteger(paramHashes[(int)AnimParams.GunActionAnimIndex], (int)gunActionAnimations.glock18c_ThreeFire);
       // animator.SetTrigger(paramHashes[(int)AnimParams.GunTriggerAnim]);
       // Debug.Log("glock18c ADS activated! : " + this.name);
    }

    // Triggers reload animation for the gun which consequently reloads the ammo in the mag of the gun once the animation event is triggered during the reload animation
    public override void Reload()
    {
        // Triggers reload animation
        PlayAnim((int)GunActionAnims.Glock18c_Reload);
    }

    // This function is ran when we equip the glock18c into our players inventory. The purpose is to initialize our pooledBullets and its relevant components as well as
    // obtaining the actual guns gameObject within the running gaame, and the camera transform of the player's main camera
    public override void Created(GameObject objectA, Transform cameraTransform)
    {
        animator = objectA.GetComponent<Animator>(); // obtains the gun animator in order to animate the gun in-game
        realGunModel = objectA; // obtains the actual gun gameObject that is within the running game
        mainCameraTransform = cameraTransform; // obtains the camera transform of the player's main camera

        // Initializes pooled series of variables (in which they will always share the same size for the sake of compatability)
        pooledBullets = new GameObject[10];
        pooledBulletsStartPositions = new Vector3[10];
        pooledTrailRenderers = new TrailRenderer[10];
        pooledBulletsEndPositions = new Vector3[10];

        pooledBulletsIndex = 0;

        for (int index = 0; index < pooledBullets.Length; index++)
        {
            // Instantiates, and insitializes all of the 'pooledBullets' and relevant components such as their trail renderers
            pooledBullets[index] = Instantiate(bulletPrefab, Vector3.zero, Quaternion.identity);
            pooledTrailRenderers[index] = pooledBullets[index].GetComponent<TrailRenderer>();
            pooledTrailRenderers[index].emitting = false;
            pooledBullets[index].SetActive(false);
        }
    }
}
