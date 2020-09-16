using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New mg61 Gun", menuName = "Item/Active/Gun/mg61")]
public class mg61_GW : BaseGun
{
    public override void FireAnim()
    {
        Debug.Log("Fired mg61 : " + this.name);
    }

    public override void SecondaryFire()
    {
        Debug.Log("Secondary fired mg61 : " + this.name);
    }
    public override void ADS()
    {
        Debug.Log("mg61 ADS activated! : " + this.name);
    }
}
