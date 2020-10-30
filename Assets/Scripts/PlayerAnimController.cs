using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles and syncs animations

// Used to easily interact with the stored Hashes for indexing
public enum ArmAnimParams { LR_Cast, L_Cast, R_Cast, LR_Emote, L_Emote, R_Emote, SpellAnimIndex, CastingAnimIndex, EmoteAnimIndex, GunAnimCatergory, MouseScroll, GunActionAnimIndex, L_GunTriggerAnim, R_GunTriggerAnim };
public enum ArmAnimState { isIdle, isAction, isInterrupt, isNoInterrupt, isAny };
public enum ArmAnimLayer { allLayers = -1, leftArm, rightArm };

/* ANIMATION PARAMETERS EXPLAINATION
 * 
 * LR_Casting : Boolean : REMOVE THIS
 * LR_Cast : Trigger : States whether both arms are to partake in a double hand signs to cast a spell
 * L_Cast : Trigger : States whether the left arm is to partake in a single hand sign to cast a spell
 * R_Cast : Trigger : States whether the right arm is to partake in a single hand sign to cast a spell (However, I may never use this)
 * 
 * LR_Emote : Trigger : ADD THIS states whether both arms are to partake in a double hand emote
 * L_Emote : Trigger : States whether the left arm is to partake in a single hand emote
 * R_Emote : Trigger : States whether the right arm is to partake in a single had emote
 * 
 * SpellAnimIndex : Int : For indexing which animation should be used when using a spell animation
 * CastingAnimIndex : Int : For indexing which animation should be used when using a casting animation that occurrs when we use handsigns in quick succession to form a powerful spell
 * EmoteAnimIndex : Int : For indexing which animaton should be used when using an emote animation
 * 
 * GunAnimCatergory : Int : For indexing which catergory of gun animations we should be using, i.e. the gun animation catergory for the gun we are holding
 * MouseScroll : Trigger : The context of MouseScroll is important for its usuage, i.e. playing the gun equipping animation when we swap guns
 * GunActionAnimIndex : Int : After specifiying our catergory of gun animations we should be using, we use this GunActionAnimIndex to index the animations within the catergory, i.e. reloading
 * 
 * L_GunTriggerAnim : Trigger : States whether the left arm is to partake in a single gun animation
 * R_GunTriggerAnim : Trigger : States whether the right arm is to partake in a signle gun animation
 * 
 */

/* ANIMATION STATES EXPLAINATION
 * 
 * isIdle : States that the arm is in a idle position, can be interrupted by any other animation
 * isAction : States whether the arm is in an active action such as firing a gun (usually transitions to isInterrupt when wanting an action to be available for interrupts after a certain amount of time)
 * isInterrupt : States whether the arm is ready to be interrupted and used especially when you want an action animation to be interrupted after a certain amount of time
 * isNoInterrupt : States whether an arm is to never be interrupted
 * isAny : Used for checking the state of an arm and bypasses CheckStateAtLayer(); and used for specific cases in which we want to issue a command regardless of an arm's state
 * 
 */

/* HOW TO ADD ANOTHER PARAMETER TO  "PlayerAnimController"
 * 1) add a parameter to the animation controller
 * 2) appended the parameter to the ArmAnimParams
 * 3) add the corressponding case into the corressponding "SetParameter" function
 */

public class PlayerAnimController : MonoBehaviour
{
    
    // *** Public variables ****
    [Header("External Interactions")]
    [Tooltip("Allows communication to animation graph to trigger animations")]
    public Animator animator;
    [Tooltip("Obtains keyboard input")]
    public KeyboardController keyboard;
    [Tooltip("Gets current arm animation state")]
    public ArmAnimObserver armAnimObserver;

    // **** Private variables ****

    // Stores the ID for each parameters name
    private readonly static int[] paramHashes =
    {
        Animator.StringToHash("LR_Cast"),
        Animator.StringToHash("L_Cast"),
        Animator.StringToHash("R_Cast"),
        Animator.StringToHash("LR_Emote"),
        Animator.StringToHash("L_Emote"),
        Animator.StringToHash("R_Emote"),
        Animator.StringToHash("SpellAnimIndex"),
        Animator.StringToHash("CastingAnimIndex"),
        Animator.StringToHash("EmoteAnimIndex"),
        Animator.StringToHash("GunAnimCatergory"),
        Animator.StringToHash("MouseScroll"),
        Animator.StringToHash("GunActionAnimIndex"),
        Animator.StringToHash("L_GunTriggerAnim"),
        Animator.StringToHash("R_GunTriggerAnim")
    };

    // Checks the state of each/both arm and returns true if the arm state is equal to one of the values in "states"
    bool CheckStateAtLayer(int layer, params int[] states)
    {
        if (states[0] == (int)ArmAnimState.isAny) return true;

        // check all layers if they are in any of the states stated in "states"

        if (layer == (int)ArmAnimLayer.allLayers)
        {
            int trueCases = 0;

            for (int x = 0; x < 2; x++)
            {
                foreach (int state in states)
                {
                    if (armAnimObserver.animState[x] == state)
                    {
                        trueCases++;
                        break;
                    }
                }
            }

            if (trueCases == 2)
                return true;
            else return false;
        }

        // check specific layer if state are in any of the states stated in "states"

        foreach (int state in states)
        {
            if (armAnimObserver.animState[layer] == state)
                return true;
        }

        return false;
    } 

    // animator.SetBool [obselete] & returns true if it could set parameter
    public bool SetParameter (int parameter, bool value, int layer, params int[] states)
    {
        if (!CheckStateAtLayer(layer, states)) // arms are unavailable to do a new animation
        {
            return false;
        }

        /*switch (parameter)
        {
            case (int)ArmAnimParams.LR_Casting:
                animator.SetBool(paramHashes[(int)ArmAnimParams.LR_Casting], value);
                break;
        }*/

        return true;
    }

    // animator.SetInteger (sets the interger of a parameter in the animation controller to "value") & returns true if it could set parameter
    public bool SetParameter (int parameter, int value, int layer, params int[] states)
    {
        if (!CheckStateAtLayer(layer, states)) // arms are unavailable to do a new animation
        {
            return false;
        }

        switch (parameter)
        {
            case (int)ArmAnimParams.SpellAnimIndex:
                animator.SetInteger(paramHashes[(int)ArmAnimParams.SpellAnimIndex], value);
                break;

            case (int)ArmAnimParams.CastingAnimIndex:
                animator.SetInteger(paramHashes[(int)ArmAnimParams.CastingAnimIndex], value);
                break;

            case (int)ArmAnimParams.EmoteAnimIndex:
                animator.SetInteger(paramHashes[(int)ArmAnimParams.EmoteAnimIndex], value);
                break;

            case (int)ArmAnimParams.GunAnimCatergory:
                animator.SetInteger(paramHashes[(int)ArmAnimParams.GunAnimCatergory], value);
                break;

            case (int)ArmAnimParams.GunActionAnimIndex:
                animator.SetInteger(paramHashes[(int)ArmAnimParams.GunActionAnimIndex], value);
                break;
        }

        return true;
    }

    // animator.SetTrigger (sets the trigger of a parameter to on in the animation controller) & returns true if it could set parameter
    public bool Trigger (int parameter, int layer, params int[] states)
    {
        if (!CheckStateAtLayer(layer, states)) // arms are unavailable to do a new animation
        {
            return false;
        }

        switch (parameter)
        {
            case (int)ArmAnimParams.LR_Cast:
                animator.SetTrigger(paramHashes[(int)ArmAnimParams.LR_Cast]);
                break;

            case (int)ArmAnimParams.L_Cast:
                animator.SetTrigger(paramHashes[(int)ArmAnimParams.L_Cast]);
                break;

            case (int)ArmAnimParams.R_Cast:
                animator.SetTrigger(paramHashes[(int)ArmAnimParams.R_Cast]);
                break;

            case (int)ArmAnimParams.L_Emote:
                animator.SetTrigger(paramHashes[(int)ArmAnimParams.L_Emote]);
                break;

            case (int)ArmAnimParams.R_Emote:
                animator.SetTrigger(paramHashes[(int)ArmAnimParams.R_Emote]);
                break;

            case (int)ArmAnimParams.MouseScroll:
                animator.SetTrigger(paramHashes[(int)ArmAnimParams.MouseScroll]);
                break;

            case (int)ArmAnimParams.L_GunTriggerAnim:
                animator.SetTrigger(paramHashes[(int)ArmAnimParams.L_GunTriggerAnim]);
                break;

            case (int)ArmAnimParams.R_GunTriggerAnim:
                animator.SetTrigger(paramHashes[(int)ArmAnimParams.R_GunTriggerAnim]);
                break;
        }

        return true;
    }




    // Combo Parsing Below
    [HideInInspector] public int pointer = 0; // Queued animations array indexer
    [HideInInspector] public int[] animations; // Queued animations for animations that occurr quickly and in succession
    [HideInInspector] public int length; // How many animations are queued

    public void AssignNew(int[] newAnimations, int newLength)
    {
        pointer = 0;
        animations = newAnimations;
        length = newLength;
    }
}
