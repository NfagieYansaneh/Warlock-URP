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
    public int Damage; // Develop randomness in damage (nah, implement critical shots tbh, but more satisfying)?

    public Transform mainCameraTransform;

    // animIndex allows me to know which animations this weapon should be using
    public enum gunAnimations{ null_animations=-1, glock18c_animations };
    public gunAnimations animIndex;

    public GameObject realGunModel; // Rename this like wtf

    public GameObject gunModel;
    public Vector3 barrelTransformOffset;
    [HideInInspector]
    public Vector3 gunTransform;

    public GameObject bulletPrefab;
    public LayerMask bulletLayerMask;
    [HideInInspector]
    public GameObject[] pooledBullets;
    [HideInInspector]
    public TrailRenderer[] pooledTrailRenderers;
    [HideInInspector]
    public Vector3[] pooledTrailEndPositions;
    [HideInInspector]
    public int pooledBulletsIndex;

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

    public virtual void Created(GameObject objectA, Transform cameraTransform)
    {

    }
}
