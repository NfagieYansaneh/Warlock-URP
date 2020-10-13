using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New glock18c", menuName = "Item/Active/Gun/glock18c")]
public class glock18c_GW : BaseGun
{
    public enum gunActionAnimations { glock18c_Reload, glock18c_Fire, glock18c_TwoFire, glock18c_ThreeFire };
    public enum AnimParams { GunActionAnimIndex, GunTriggerAnim };
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

    public override void FireAnim(int animation)
    {
        animator.SetInteger(paramHashes[(int)AnimParams.GunActionAnimIndex], (int)animation);
        animator.SetTrigger(paramHashes[(int)AnimParams.GunTriggerAnim]);
        Debug.Log("Fired glock18c : " + this.name);
    }

    public override void FireRaycast()
    {
        Vector3 end = mainCameraTransform.position + (mainCameraTransform.forward * 10f);
        Ray ray = new Ray(mainCameraTransform.position, mainCameraTransform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 200f, bulletLayerMask)) // Optimize with a layer mask & make distance short as needed
        {
            if (hit.collider.tag == "Enemy")
            {
                hit.collider.gameObject.GetComponent<GenericEnemyHandler>().Hit(Damage);
            }

            pooledTrailEndPositions[pooledBulletsIndex] = hit.point;
        }
        else
        {
            pooledTrailEndPositions[pooledBulletsIndex] = end;
        }



        pooledBullets[pooledBulletsIndex].transform.position = realGunModel.transform.position +
            (realGunModel.transform.forward * barrelTransformOffset.z) + (realGunModel.transform.up * barrelTransformOffset.y) + (realGunModel.transform.right * barrelTransformOffset.x);

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

    public override void SecondaryFire()
    {
        Debug.Log("Secondary fired glock18c : " + this.name);
    }
    public override void ADS()
    {
        animator.SetInteger(paramHashes[(int)AnimParams.GunActionAnimIndex], (int)gunActionAnimations.glock18c_ThreeFire);
        animator.SetTrigger(paramHashes[(int)AnimParams.GunTriggerAnim]);
        Debug.Log("glock18c ADS activated! : " + this.name);
    }

    public override void Reload(int x)
    {
        animator.SetInteger(paramHashes[(int)AnimParams.GunActionAnimIndex], x);
        animator.SetTrigger(paramHashes[(int)AnimParams.GunTriggerAnim]);
    }

    public override void Created(GameObject objectA, Transform cameraTransform)
    {
        animator = objectA.GetComponent<Animator>();
        realGunModel = objectA;
        mainCameraTransform = cameraTransform;

        pooledBullets = new GameObject[10];
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
