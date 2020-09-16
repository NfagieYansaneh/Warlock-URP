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
        Vector3 end = fpCamera.position + (fpCamera.forward * range);
        Debug.DrawLine(fpCamera.position, end, color, 1f);
        Ray ray = new Ray(fpCamera.position, end);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, range))
        {
            Debug.Log(hit.collider.gameObject.name);
        }
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
        fpCamera = transform;
    }
}
