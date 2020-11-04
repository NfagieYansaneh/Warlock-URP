using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Purpose of BaseGun.cs
 * 
 * BaseGun.cs is to be the template that every other gun is based off. It stores a series of virtual functions and general variables (ex. maxAmmoInMag)
 * that is to be overwritten (if neccessary) by every other gun script when developing a new gun
 * 
 */

public class BaseGun : ScriptableObject
{
    //[Header("Basic elements")]
    //[Space(10)]

    [Header("UI elements")]
    [TextArea(10, 15)]
    public string basicDescription; // basic description of gun (not actually utilised in game as of now)
    public Sprite uiImage; // the 2d gun icon that is to be displayed on the player's Ui (not actually utilised in game as of now)
    public Color color; // placeholder basic 2d gun image that merely displays the gun as a solid color since I do not have 2d gun icons as of now
    [Space(10)]

    [Header("Basic gun model elements")]
    public GameObject gunModel; // the actual 3d gun model that is to be shown in our player's hands
    
    // used to offset the bullets so they appear to come out of the gun's barrel (this will later be scrapped as I can use blender (3d modelling software) to attach a transform to the end of the barrel)
    public Vector3 barrelTransformOffset;
    public enum GunAnimations { null_animations = -1, glock18c_animations };
    public GunAnimations gunAnimCatergory; // gunAnimCatergory allows me to define which set of animations this weapon should be using (such as smg animations, or shotgun animations)

    [Space(10)]

    // Below contains the contents required for firing bullets and handling their bullet trails
    
    [Header("Bullet elements")]
    public GameObject bulletPrefab; // stores bullet object which is instantiated upon fire
    public AudioSource bulletAudioSource; // sound that plays upon bullet fire (not actually utilised in game as of now)
    public LayerMask bulletLayerMask; // stores masking data as to know which objects the bullet can collide with
    [HideInInspector]

    // Instianting and destroying bullets constantly is very quite performance intensive. Thus, we instantiated all our bullets once and then when we wish to fire
    // bullets, we just enable one of those bullet objects with our pool of instantiated bullets. When bullets collide, we just disable them

    public GameObject[] pooledBullets; // our pool of instantiated bullets
    [HideInInspector]
    public TrailRenderer[] pooledTrailRenderers; // our pool of trail renderers to render the bullet trails of our instantiated bullets
    [HideInInspector]
    public Vector3[] pooledBulletsEndPositions; // our pool of bullet end 
    [HideInInspector]
    public Vector3[] pooledBulletsStartPositions; // our pool of bullet start positions for when bullets become fired
    [HideInInspector]
    public int pooledBulletsIndex; // this value is merely a counter so we can loop through all the pooled bullets

    [Header("Ammo, Damage & Damage RNG elements")]
    public float baseBulletSpeed;
    //public float maxBulletSpeed;
    public int ammoInMag;
    public int maxAmmoInMag;
    [Range(0f, 10f)]
    public int damage; // bullet damage
    // Develop randomness in damage (nah, implement critical shots tbh, but more satisfying)?
    [Space(10)]

    // Values that are definied at runtime
    // ?????????????????????????????????????????????????????????????????????????????????????
    // ?????????????????????????????????????????????????????????????????????????????????????
    // ?????????????????????????????????????????????????????????????????????????????????????
    // ?????????????????????????????????????????????????????????????????????????????????????
    public Transform mainCameraTransform; // ?????????????????????????????????????????????????????????????????????????????????????
    public GameObject realGunModel; // ?????????????????????????????????????????????????????????????????????????????????????

    // Guns will have a dictionary of animations for firing, reloading, and activiating their abilities. Thus, these values below indexes towards which animations they should be doing
    // These values are defined beforehand, and should be only be changed if a gun was to be upgraded
    [HideInInspector]
    public int fireAnimIndex; // this value will let me index into which animation is our firing animation
    [HideInInspector]
    public int reloadAnimIndex; // this value will let me index into which animation is our reload animation

    // basic command to get the gun to play an animation (meant to be defined for that given gun script)
    public virtual void PlayAnim(int animation)
    {

    }

    // basic command to fire a bullet for our gun from its bullet pool (meant to be defined for that given gun script)
    public virtual void FireBulletFromPool()
    {

    }

    // basic command to disable a bullet for our gun from its bullet pool when bullets collides against objects/enemies (meant to be defined for that given gun script)
    public virtual void DisableBulletFromPool(int index)
    {

    }

    // basic command to handle a guns primary fire function (meant to be defined for that given gun script)
    public virtual void PrimaryFire(PlayerAnimController playerAnimController)
    {

    }

    // basic command to handle a guns secondary fire function (meant to be defined for that given gun script)
    public virtual void SecondaryFire(PlayerAnimController playerAnimController)
    {

    }

    // basic function that runs when bullets collides with enemies
    public virtual void OnEnemyImpact()
    {

    }

    // basic command to cause a player to aim down the sights of a gun
    public virtual void ADS()
    {

    }

    // basic command to play a guns reload animation and refill the ammoInMag as a consequence (meant to be defined for that given gun script)
    public virtual void Reload()
    {

    }

    // basic function that runs when a gun is "created" by being equipped and placed within our player's inventory
    public virtual void Created(GameObject objectA, Transform cameraTransform)
    {

    }
}
