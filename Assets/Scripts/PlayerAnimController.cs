using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles and syncs animations

// Used to easily interact with the stored Hashes for indexing
public enum AnimParams { LR_Casting, LR_Cast, L_Cast, R_Cast, L_Emote, R_Emote, CastingIndex, EmoteIndex, GunAnimIndex, MouseScroll, GunActionAnimIndex, GunTriggerAnim};
public enum AnimState { isIdle, isAction, isNo_interrupt, isAny };
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
        Animator.StringToHash("GunTriggerAnim")
    };

    bool CheckStateAtLayer(int layer, int state)
    {
        if (state == (int)AnimState.isAny) return true;

        if (layer == (int)AnimLayer.allLayers)
        {
            for (int x = 0; x < 2; x++)
            {
                if (armAnimObserver.animState[x] != state)
                {
                    Debug.Log(armAnimObserver.animState[x] + " != " + state);
                    return false;
                }
            }

            return true;
        }

        if (armAnimObserver.animState[layer] == state)
        {
            return true;
        }
        else return false;
    }

    /* change SetParameter to return bools if it has set the parameter */

    // animator.SetBool
    public bool SetParameter (int parameter, bool value, int layer, int state)
    {
        if (!CheckStateAtLayer(layer, state)) // arms are unavailable to do a new animation
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
    public bool SetParameter (int parameter, int value, int layer, int state)
    {
        if (!CheckStateAtLayer(layer, state)) // arms are unavailable to do a new animation
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
    public bool Trigger (int parameter, int layer, int state)
    {
        if (!CheckStateAtLayer(layer, state)) // arms are unavailable to do a new animation
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

            case (int)AnimParams.GunTriggerAnim:
                animator.SetTrigger(paramHashes[(int)AnimParams.GunTriggerAnim]);
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
