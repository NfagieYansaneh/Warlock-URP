using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Purpose of BoosterSettings.cs
 * 
 * BoosterSettings.cs stores basic booster data for the booster balls. Upon contact, we grab the boosterDirection
 * and boosterDistance in order to know where the player should be getting launched and for how far. However, we have
 * no boosters in our scene since they were in my previous build and feedback as led to be getting rid of the ideas of
 * boosters
 */

public class BoosterSettings : MonoBehaviour
{
    public Vector3 boosterDirection;
    public float boosterDistance;
}
