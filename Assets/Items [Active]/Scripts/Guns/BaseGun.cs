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
    public float RPM;

    // animIndex allows me to know which animations this weapon should be using
    public enum gunAnimations{ glock18c_animations, null_animations};
    public gunAnimations animIndex;

    public GameObject gunModel;
    public Vector3 transformOffset;
    public Vector3 barrelEndOffset;
    public Vector3 rotationOffset;

    public virtual void Fire()
    {

    }

    public virtual void SecondaryFire()
    {

    }

    public virtual void ADS()
    {

    }
}
