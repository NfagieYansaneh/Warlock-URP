using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Purpose of PlayerSettings.cs
 * 
 * PlayerSettings.cs stores basic settins for player such as the player's steep, maximum slope angle in which the player
 * can walk up, etc.
 */

[CreateAssetMenu(fileName = "New Player Settings", menuName = "Player Settings")]
public class PlayerSettings : ScriptableObject
{
    public float speed;
    public float gravity;
    public float jumpHeight;
    public float stepOffset;
    public float stepMaxAngle;
    public float slopeMaxAngle;
    public float maxJmps; // number of jumps the player can do in succession
    public float x; // still trying to come up with a more specific, intuitive name for this variable

    // the ghost series of values exist because my movement is a two part system consisting of...
    // 'kinematicVelocity' which is responsible for 'snappy' movements
    // 'ghostVelocity' which is responsible for the lingering slide or vertical velocity that changes due to gravity experienced when jumping

    [Range(0f, 1f)]
    public float ghostCollidingFriction; // stores the sliding effect value. Adjusts this will adjust our player's natural sliding
    [Range(0f, 10f)]
    public float ghostSlideScalar; // ghostSlideScalar is used to increase the sliding effect for the player when we are croutching
    [Range(0f, 0.5f)]
    public float boosterInputDelay; // prevents player from boosting off a world space booster object every frame
    public float dodgeBoostDelay; // prevents player from slide boosting without delay
    [Range(0f, 1f)]
    public float kioskActivateDelay; // kioskActivateDelay determines how long we have to hold the interact button to activate the kiosk
    [Range(0f, 1f)]
    public float basicGunPickupDelay; // basicGunPickupDelay determines how long we have to hold the interact button to pick up a gun
}
