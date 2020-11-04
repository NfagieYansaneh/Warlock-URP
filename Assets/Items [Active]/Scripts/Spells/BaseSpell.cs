using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Purpose of BaseSpell.cs
 * 
 * BaseSpell.cs is to be the template that every other spell is based off. It stores a series of virtual functions and general variables (ex. audioSource)
 * that is to be overwritten (if neccessary) by every other spell script when developing a new spell
 * 
 */

public class BaseSpell : ScriptableObject
{
    [Header("UI elements")]
    [TextArea(10, 15)]
    public string basicDescription; // basic description of gun (not actually utilised in game as of now)
    public AudioSource audioSource; // sound that plays upon spell casting (not actually utilised in game as of now)
    public Sprite image; // the 2d spell icon that is to be displayed on the player's Ui (not actually utilised in game as of now)
    public Color color; // placeholder basic 2d spell image that merely displays the gun as a solid color since I do not have 2d gun icons as of now

    [Header("Basic spell elements")]

    public bool isOneHanded; // does the spell cast only need one hand to do so?

    public enum Anims { Null=-1, Igni }; // list of one handed animation casts
    public Anims animIndex; // this value will let me index into which animation is our one handed spell animation (will only be used if the spell is one handed)

    public enum AnimsDouble { Null, Ox, Horse, Monkey, Ram };
    public AnimsDouble[] animDoubleIndexes; // this value will let me index into which animation is our double handed spell animation (will only be used if the spell is NOT one handed)

    // basic command to Use/Activate a spell (meant to be defined for by every spell script)
    public virtual void Use()
    {

    }

    // Some spells will display their own unique crosshair in-game. This function is for the responsive and reactive crosshairs
    public virtual void AlternativeCrosshairActive()
    {

    }

    // This function is for unresponsive and unreactive crosshairs that a spell could provide
    public virtual void AlternativeCrosshairPassive()
    {

    }
}
