using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSpell : ScriptableObject
{
    [TextArea(10, 15)]
    public string description;
    public AudioSource audioSource;
    public Sprite image;
    public Color color;

    public bool isOneHanded; // One handed spell cast

    public enum Anims { Null=-1, Igni }; // list of one handed animation casts
    public Anims animIndex; 

    public enum AnimsDouble { Null, Ox, Horse, Monkey, Ram };
    public AnimsDouble[] animDoubleIndexes;

    public virtual void Use()
    {

    }

    public virtual void AlternativeCrosshairActive()
    {

    }

    public virtual void AlternativeCrosshairPassive()
    {

    }
}
