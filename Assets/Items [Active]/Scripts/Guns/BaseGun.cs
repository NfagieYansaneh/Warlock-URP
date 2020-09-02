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
