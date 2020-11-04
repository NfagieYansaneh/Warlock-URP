using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Purpose of NullGun_GW.cs
 * 
 * NullGun_GW.cs contains "GW" in its name which means Gun Weapon 
 * 
 * NullGun_GW.cs is our "NULL" when it comes to guns. Therefore, if we only have one gun in our player's inventory
 * then that second empty slot in our player's gun inventory will be actually a NullGun_GW spot
 * 
 */

[CreateAssetMenu(fileName = "New Null Gun", menuName = "Item/Active/Gun/Null")]
public class NullGun_GW : BaseGun
{
    public override void PrimaryFire(PlayerAnimController playerAnimController)
    {
        Debug.Log("Fired NULL GUN");
    }

    public override void SecondaryFire(PlayerAnimController playerAnimController)
    {
        Debug.Log("Secondary fired NULL GUN");
    }
    public override void ADS()
    {
        Debug.Log("NULL GUN ADS activated!");
    }
}
