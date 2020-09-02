using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPickup : MonoBehaviour
{
    public BaseGun gunSlot;

    private void Start()
    {
        gameObject.tag = "GunPickup";
    }
}
