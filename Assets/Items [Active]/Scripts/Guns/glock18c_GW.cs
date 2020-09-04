using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New glock18c", menuName = "Item/Active/Gun/glock18c")]
public class glock18c_GW : BaseGun
{
    public enum gunActionAnimations { glock18c_Reload };
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

    public override void Fire()
    {
        Debug.Log("Fired glock18c : " + this.name);
    }

    public override void SecondaryFire()
    {
        Debug.Log("Secondary fired glock18c : " + this.name);
    }
    public override void ADS()
    {
        Debug.Log("glock18c ADS activated! : " + this.name);
    }

    public override void Reload(int x)
    {
        animator.SetInteger(paramHashes[(int)AnimParams.GunActionAnimIndex], x);
        animator.SetTrigger(paramHashes[(int)AnimParams.GunTriggerAnim]);
    }
}
