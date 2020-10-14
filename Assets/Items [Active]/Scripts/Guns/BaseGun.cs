﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGun : ScriptableObject
{
    //[Header("Basic elements")]
    //[Space(10)]

    [Header("UI elements")]
    [TextArea(10, 15)]
    public string basicDescription;
    public Sprite uiImage;
    public Color color;
    [Space(10)]

    [Header("Basic gun model elements")]
    public GameObject gunModel;
    public Vector3 barrelTransformOffset;
    public enum gunAnimations { null_animations = -1, glock18c_animations };
    public gunAnimations gunAnimGroup; // animGroup allows me to know which animations this weapon should be using
    [Space(10)]

    // Below contains the contents required for firing bullets and handling their bullet trails
    
    [Header("Bullet elements")]
    public GameObject bulletPrefab; // stores bullet object which is instantiated upon fire
    public AudioSource bulletAudioSource;
    public LayerMask bulletLayerMask; // stores masking data as to know which objects the bullet can collide with
    [HideInInspector]
    public GameObject[] pooledBullets;
    [HideInInspector]
    public TrailRenderer[] pooledTrailRenderers;
    [HideInInspector]
    public Vector3[] pooledTrailEndPositions; // REMOVE THIS, WE ARE NOT RAYCASTING ANYMORE
    [HideInInspector]
    public int pooledBulletsIndex; // this value is merely a counter so we can loop through all the pooled bullets

    [Header("Ammo, Damage & Damage RNG elements")]
    public int ammoInMag;
    public int maxAmmoInMag;
    [Range(0f, 10f)]
    public int Damage; // Develop randomness in damage (nah, implement critical shots tbh, but more satisfying)?
    [Space(10)]

    // Values that are definied at runtime
    public Transform mainCameraTransform;
    public GameObject realGunModel;

    // Guns will have a dictionary of animations for firing, reloading, and activiating their abilities. Thus, these values below indexes towards which animations they should be doing
    // These values are defined before and can only be changed if a gun was to be upgraded
    [HideInInspector]
    public int fireAnimIndex; // this value will let us index into which animation is our firing animation
    [HideInInspector]
    public int reloadAnimIndex; // this value will let us index into which animation is our reload animation

    public virtual void FireAnim(int animation)
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
