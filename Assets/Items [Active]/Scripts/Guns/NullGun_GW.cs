using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Null Gun", menuName = "Item/Active/Gun/Null")]
public class NullGun_GW : BaseGun
{
    public override void Fire()
    {
        Debug.Log("Fired NULL GUN");
    }

    public override void SecondaryFire()
    {
        Debug.Log("Secondary fired NULL GUN");
    }
    public override void ADS()
    {
        Debug.Log("NULL GUN ADS activated!");
    }
}
