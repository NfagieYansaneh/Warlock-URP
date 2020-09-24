using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Settings", menuName = "Player Settings")]
public class PlayerSettings : ScriptableObject
{
    public float speed;
    public float gravity;
    public float jumpHeight;
    public float stepOffset;
    public float stepMaxAngle;
    public float slopeMaxAngle;
    public float maxJmps;
    public float x; // still trying to come up with a more specific, intuitive name for this variable
    [Range(0f, 1f)]
    public float ghostCollidingFriction;
    [Range(0f, 10f)]
    public float ghostSlideScalar;
    [Range(0f, 0.5f)]
    public float boosterInputDelay;
    public float dodgeBoostDelay;
    [Range(0f, 1f)]
    public float kioskActivateDelay;
}
