using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutputAnimTag : StateMachineBehaviour
{
    public ArmAnimObserver animObserver;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animObserver.UpdateAnimState(stateInfo, layerIndex);
    }
}
