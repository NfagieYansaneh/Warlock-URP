using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGun : ScriptableObject
{
    [TextArea(10, 15)]
    public string description;
    public AudioSource audioSource;
    public Sprite image;
    public Color color;
    //public float RPM;
    public int ammo;
    public int ammoInMag;
    public int maxAmmoInMag;

    [Range(0f, 10f)]
    public float range;
    public Transform fpCamera;

    // animIndex allows me to know which animations this weapon should be using
    public enum gunAnimations{ null_animations=-1, glock18c_animations };
    public gunAnimations animIndex;

    public GameObject gunModel;
    public Vector3 transformOffset;
    public Vector3 barrelEndOffset;
    public Vector3 rotationOffset;

    public virtual void FireAnim()
    {

    }

    public virtual void FireRaycast()
    {

    }

    public virtual void SecondaryFire()
    {

    }

    public virtual void ADS()
    {

    }

    public virtual void Reload(int x)
    {

    }

    public virtual void Created(GameObject objectA, Transform transform)
    {

    }
}
