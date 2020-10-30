using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New glock18c", menuName = "Item/Active/Gun/glock18c")]
public class glock18c_GW : BaseGun
{
    public enum GunActionAnims { Glock18c_Reload, Glock18c_Fire, Glock18c_TwoFire, Glock18c_ThreeFire };
    public enum GunAnimParams { GunActionAnimIndex, GunTriggerAnim };

    public enum GunMods { SingleShot, TwoShotBurst, ThreeShotBurst };
    public GunMods activeGunMod;

    public Animator animator;

    private void OnEnable()
    {
        animator = gunModel.GetComponent<Animator>();
    }

    private readonly static int[] paramHashes =
    {
        Animator.StringToHash("GunActionAnimIndex"),
        Animator.StringToHash("GunTriggerAnim")
    };

    public override void PlayAnim(int animation)
    {
        animator.SetInteger(paramHashes[(int)GunAnimParams.GunActionAnimIndex], (int)animation);
        animator.SetTrigger(paramHashes[(int)GunAnimParams.GunTriggerAnim]);
        Debug.Log("Fired glock18c : " + this.name);
    }

    public override void FireBulletFromPool()
    {
        Vector3 end = mainCameraTransform.position + (mainCameraTransform.forward * 10f);
        Ray ray = new Ray(mainCameraTransform.position, mainCameraTransform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 200f, bulletLayerMask)) // Optimize with a layer mask & make distance short as needed
        {

            pooledTrailEndPositions[pooledBulletsIndex] = hit.point;
        }
        else
        {
            pooledTrailEndPositions[pooledBulletsIndex] = end;
        }



        pooledBullets[pooledBulletsIndex].transform.position = realGunModel.transform.position +
            (realGunModel.transform.forward * barrelTransformOffset.z) + (realGunModel.transform.up * barrelTransformOffset.y) + (realGunModel.transform.right * barrelTransformOffset.x);

        pooledBulletsStartPositions[pooledBulletsIndex] = pooledBullets[pooledBulletsIndex].transform.position;

        if (Physics.Raycast(ray, 5f)) // For when objects are close
            pooledBullets[pooledBulletsIndex].transform.LookAt(end);
        else
        {
            pooledBullets[pooledBulletsIndex].transform.LookAt(hit.point);
        }

        pooledBullets[pooledBulletsIndex].SetActive(true);
        pooledTrailRenderers[pooledBulletsIndex].emitting = true;
        if (pooledBulletsIndex == pooledBullets.Length - 1) pooledBulletsIndex = 0;
        else pooledBulletsIndex++;
    }

    public override void DisableBulletFromPool(int index)
    {
        pooledTrailRenderers[index].emitting = false;
        pooledBullets[index].SetActive(false);
    }

    public override void PrimaryFire(PlayerAnimController playerAnimController)
    {
        if (ammoInMag > 0)
            switch (activeGunMod)
            {
                case GunMods.SingleShot:

                    if (playerAnimController.SetParameter((int)ArmAnimParams.GunActionAnimIndex, (int)GunActionAnims.Glock18c_Fire, (int)ArmAnimLayer.rightArm,
                        (int)ArmAnimState.isIdle, (int)ArmAnimState.isInterrupt))
                    {
                        playerAnimController.Trigger((int)ArmAnimParams.R_GunTriggerAnim, (int)ArmAnimLayer.rightArm, (int)ArmAnimState.isAny);
                        PlayAnim((int)GunActionAnims.Glock18c_Fire);
                    }
                    break;

                case GunMods.TwoShotBurst:
                    if (ammoInMag >= 2)
                    {
                        if (playerAnimController.SetParameter((int)ArmAnimParams.GunActionAnimIndex, (int)GunActionAnims.Glock18c_TwoFire, (int)ArmAnimLayer.rightArm,
                            (int)ArmAnimState.isIdle, (int)ArmAnimState.isInterrupt))
                        {
                            playerAnimController.Trigger((int)ArmAnimParams.R_GunTriggerAnim, (int)ArmAnimLayer.rightArm, (int)ArmAnimState.isAny);
                            PlayAnim((int)GunActionAnims.Glock18c_TwoFire);
                        }
                    }
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
                    if (ammoInMag >= 3)
                    {
                        if (playerAnimController.SetParameter((int)ArmAnimParams.GunActionAnimIndex, (int)GunActionAnims.Glock18c_ThreeFire, (int)ArmAnimLayer.rightArm,
                            (int)ArmAnimState.isIdle, (int)ArmAnimState.isInterrupt))
                        {
                            playerAnimController.Trigger((int)ArmAnimParams.R_GunTriggerAnim, (int)ArmAnimLayer.rightArm, (int)ArmAnimState.isAny);
                            PlayAnim((int)GunActionAnims.Glock18c_ThreeFire);
                        }
                    }
                    else if (ammoInMag == 2)
                    {
                        if (playerAnimController.SetParameter((int)ArmAnimParams.GunActionAnimIndex, (int)GunActionAnims.Glock18c_TwoFire, (int)ArmAnimLayer.rightArm,
                            (int)ArmAnimState.isIdle, (int)ArmAnimState.isInterrupt))
                        {
                            playerAnimController.Trigger((int)ArmAnimParams.R_GunTriggerAnim, (int)ArmAnimLayer.rightArm, (int)ArmAnimState.isAny);
                            PlayAnim((int)GunActionAnims.Glock18c_TwoFire);
                        }
                    }
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

    public override void SecondaryFire(PlayerAnimController playerAnimController)
    {
        // Below is just for debugging sake
        // also, in release, this gun has no secondary fire

        if (ammoInMag >= 3)
        {
            if (playerAnimController.SetParameter((int)ArmAnimParams.GunActionAnimIndex, (int)GunActionAnims.Glock18c_ThreeFire, (int)ArmAnimLayer.rightArm,
                (int)ArmAnimState.isIdle, (int)ArmAnimState.isInterrupt))
            {
                playerAnimController.Trigger((int)ArmAnimParams.R_GunTriggerAnim, (int)ArmAnimLayer.rightArm, (int)ArmAnimState.isAny);
                PlayAnim((int)GunActionAnims.Glock18c_ThreeFire);
            }
        }
        else if (ammoInMag == 2)
        {
            if (playerAnimController.SetParameter((int)ArmAnimParams.GunActionAnimIndex, (int)GunActionAnims.Glock18c_TwoFire, (int)ArmAnimLayer.rightArm,
                (int)ArmAnimState.isIdle, (int)ArmAnimState.isInterrupt))
            {
                playerAnimController.Trigger((int)ArmAnimParams.R_GunTriggerAnim, (int)ArmAnimLayer.rightArm, (int)ArmAnimState.isAny);
                PlayAnim((int)GunActionAnims.Glock18c_TwoFire);
            }
        }
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

    public override void ADS()
    {
       // animator.SetInteger(paramHashes[(int)AnimParams.GunActionAnimIndex], (int)gunActionAnimations.glock18c_ThreeFire);
       // animator.SetTrigger(paramHashes[(int)AnimParams.GunTriggerAnim]);
       // Debug.Log("glock18c ADS activated! : " + this.name);
    }

    public override void Reload()
    {
        // Triggers reload animation
        PlayAnim((int)GunActionAnims.Glock18c_Reload);
    }

    public override void Created(GameObject objectA, Transform cameraTransform)
    {
        animator = objectA.GetComponent<Animator>();
        realGunModel = objectA;
        mainCameraTransform = cameraTransform;

        pooledBullets = new GameObject[10];
        pooledBulletsStartPositions = new Vector3[10];
        pooledTrailRenderers = new TrailRenderer[10];
        pooledTrailEndPositions = new Vector3[10];

        pooledBulletsIndex = 0;

        for (int index = 0; index < pooledBullets.Length; index++)
        {
            pooledBullets[index] = Instantiate(bulletPrefab, Vector3.zero, Quaternion.identity);
            pooledTrailRenderers[index] = pooledBullets[index].GetComponent<TrailRenderer>();
            pooledTrailRenderers[index].emitting = false;
            pooledBullets[index].SetActive(false);
        }
    }
}
