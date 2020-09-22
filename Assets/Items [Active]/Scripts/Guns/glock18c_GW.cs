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

    public override void FireAnim()
    {
        animator.SetInteger(paramHashes[(int)AnimParams.GunActionAnimIndex], (int)gunActionAnimations.glock18c_Fire);
        animator.SetTrigger(paramHashes[(int)AnimParams.GunTriggerAnim]);
        Debug.Log("Fired glock18c : " + this.name);
    }

    public override void FireRaycast()
    {
        Vector3 end = fpCamera.position + (fpCamera.forward * 100f);
        Ray ray = new Ray(fpCamera.position, end);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, range))
        {
            Debug.Log(hit.collider.gameObject.name);
        }

        pooledBullets[pooledBulletsIndex].transform.position = realGunModel.transform.position +
            (realGunModel.transform.forward * barrelTransformOffset.z) + (realGunModel.transform.up * barrelTransformOffset.y) + (realGunModel.transform.right * barrelTransformOffset.x);

        pooledBullets[pooledBulletsIndex].transform.LookAt(end);
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

    public override void Created(GameObject objectA, Transform transform)
    {
        animator = objectA.GetComponent<Animator>();
        realGunModel = objectA;
        fpCamera = transform;

        pooledBullets = new GameObject[10];
        pooledTrailRenderers = new TrailRenderer[10];
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
