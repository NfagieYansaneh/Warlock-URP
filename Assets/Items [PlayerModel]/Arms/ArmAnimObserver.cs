using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Arm Animation Observer", menuName = "Systems/Arm Animation Observer")]
public class ArmAnimObserver : ScriptableObject
{
    private readonly static int[] tagHashes =
    {
        Animator.StringToHash("Idle"),
        Animator.StringToHash("Action"),
        Animator.StringToHash("No_interrupt")
    };

    [HideInInspector]
    public int[] animState = new int[2];

    public void UpdateAnimState(AnimatorStateInfo stateInfo, int layerIndex)
    {
        for (int x=0; x<tagHashes.Length; x++)
        {
            if(stateInfo.tagHash == tagHashes[x])
            {
                animState[layerIndex] = x;
                Debug.Log("animState[" + layerIndex + "] is " + x);
            }
        }
    }
}
