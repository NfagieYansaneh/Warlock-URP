using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Purpose of ArmControllerLinker.cs
 * 
 * ArmControllerLinker.cs is so ArmComboParser.cs can acquire the player's animation controller becuase
 * ArmComboParser.cs is a StateMachineBehaviour script, and because of the nature of StateMachineBehaviour scripts, it
 * we must use a MonoBehaviour script to provide ArmBomoParser.cs the player's animation controller at run time
 * 
 */

public class ArmControllerLinker : MonoBehaviour
{
    public PlayerAnimController AnimController;
}
