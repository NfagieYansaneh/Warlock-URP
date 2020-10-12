using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Arm Animation Observer", menuName = "Systems/Arm Animation Observer")]
public class ArmAnimObserver : ScriptableObject
{

    public delegate void updateDebugTextIfEnabledDelegateFunction();
    public updateDebugTextIfEnabledDelegateFunction updateDebugTextIfEnabled;

    private readonly static int[] tagHashes =
    {
        Animator.StringToHash("Idle"),
        Animator.StringToHash("Action"),
        Animator.StringToHash("GunFiring"),
        Animator.StringToHash("NoInterrupt")
    };

    [HideInInspector]
    public int[] animState = { 0, 0 };

    public void UpdateAnimState(AnimatorStateInfo stateInfo, int layerIndex)
    {
        for (int x=0; x<tagHashes.Length; x++)
        {
            if(stateInfo.tagHash == tagHashes[x])
            {
                animState[layerIndex] = x;
                updateDebugTextIfEnabled();
            }
        }
    }
}
