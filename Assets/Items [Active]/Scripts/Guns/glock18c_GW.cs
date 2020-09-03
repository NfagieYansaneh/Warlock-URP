using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New glock18c", menuName = "Item/Active/Gun/glock18c")]
public class glock18c_GW : BaseGun
{
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
}
