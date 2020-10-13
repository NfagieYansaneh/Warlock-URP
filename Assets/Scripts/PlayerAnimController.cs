using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles and syncs animations

// Used to easily interact with the stored Hashes for indexing
public enum AnimParams { LR_Casting, LR_Cast, L_Cast, R_Cast, L_Emote, R_Emote, CastingIndex, EmoteIndex, GunAnimIndex, MouseScroll, GunActionAnimIndex, L_GunTriggerAnim, R_GunTriggerAnim };
public enum AnimState { isIdle, isAction, isInterrupt, isNoInterrupt, isAny };
public enum AnimLayer { allLayers=-1, leftArm, rightArm };

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
        Animator.StringToHash("LR_Casting"),
        Animator.StringToHash("LR_Cast"),
        Animator.StringToHash("L_Cast"),
        Animator.StringToHash("R_Cast"),
        Animator.StringToHash("L_Emote"),
        Animator.StringToHash("R_Emote"),
        Animator.StringToHash("CastingIndex"),
        Animator.StringToHash("EmoteIndex"),
        Animator.StringToHash("GunAnimIndex"),
        Animator.StringToHash("MouseScroll"),
        Animator.StringToHash("GunActionAnimIndex"),
        Animator.StringToHash("L_GunTriggerAnim"),
        Animator.StringToHash("R_GunTriggerAnim")
    };

    bool CheckStateAtLayer(int layer, params int[] states)
    {
        if (states[0] == (int)AnimState.isAny) return true;

        // check all layers if they are in any of the states stated in "states"

        if (layer == (int)AnimLayer.allLayers)
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

    /* change SetParameter to return bools if it has set the parameter */

    // animator.SetBool
    public bool SetParameter (int parameter, bool value, int layer, params int[] states)
    {
        if (!CheckStateAtLayer(layer, states)) // arms are unavailable to do a new animation
        {
            return false;
        }

        switch (parameter)
        {
            case (int)AnimParams.LR_Casting:
                animator.SetBool(paramHashes[(int)AnimParams.LR_Casting], value);
                break;
        }

        return true;
    }

    // animator.SetInteger
    public bool SetParameter (int parameter, int value, int layer, params int[] states)
    {
        if (!CheckStateAtLayer(layer, states)) // arms are unavailable to do a new animation
        {
            return false;
        }

        switch (parameter)
        {
            case (int)AnimParams.CastingIndex:
                animator.SetInteger(paramHashes[(int)AnimParams.CastingIndex], value);
                break;

            case (int)AnimParams.EmoteIndex:
                animator.SetInteger(paramHashes[(int)AnimParams.EmoteIndex], value);
                break;

            case (int)AnimParams.GunAnimIndex:
                animator.SetInteger(paramHashes[(int)AnimParams.GunAnimIndex], value);
                break;

            case (int)AnimParams.GunActionAnimIndex:
                animator.SetInteger(paramHashes[(int)AnimParams.GunActionAnimIndex], value);
                break;
        }

        return true;
    }

    // animator.SetTrigger
    public bool Trigger (int parameter, int layer, params int[] states)
    {
        if (!CheckStateAtLayer(layer, states)) // arms are unavailable to do a new animation
        {
            return false;
        }

        switch (parameter)
        {
            case (int)AnimParams.LR_Cast:
                animator.SetTrigger(paramHashes[(int)AnimParams.LR_Cast]);
                break;

            case (int)AnimParams.L_Cast:
                animator.SetTrigger(paramHashes[(int)AnimParams.L_Cast]);
                break;

            case (int)AnimParams.R_Cast:
                animator.SetTrigger(paramHashes[(int)AnimParams.R_Cast]);
                break;

            case (int)AnimParams.L_Emote:
                animator.SetTrigger(paramHashes[(int)AnimParams.L_Emote]);
                break;

            case (int)AnimParams.R_Emote:
                animator.SetTrigger(paramHashes[(int)AnimParams.R_Emote]);
                break;

            case (int)AnimParams.MouseScroll:
                animator.SetTrigger(paramHashes[(int)AnimParams.MouseScroll]);
                break;

            case (int)AnimParams.L_GunTriggerAnim:
                animator.SetTrigger(paramHashes[(int)AnimParams.L_GunTriggerAnim]);
                break;

            case (int)AnimParams.R_GunTriggerAnim:
                animator.SetTrigger(paramHashes[(int)AnimParams.R_GunTriggerAnim]);
                break;
        }

        return true;
    }

    // Combo Parsing Below
    [HideInInspector] public int pointer = 0;
    [HideInInspector] public int[] animations;
    [HideInInspector] public int length;

    public void AssignNew(int[] newAnimations, int newLength)
    {
        pointer = 0;
        animations = newAnimations;
        length = newLength;
    }
}
