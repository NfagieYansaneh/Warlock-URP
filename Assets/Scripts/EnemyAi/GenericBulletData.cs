using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Purpose of GenericBulletData.cs
 * 
 * GenericBulletData.cs stores the damage value of an enemies bullet in motion so that when it collides with the
 * player, we can subtract the players health the damage value stored here
 */

public class GenericBulletData : MonoBehaviour
{
    public int damage;
}
