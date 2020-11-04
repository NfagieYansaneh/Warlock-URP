using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Purpose of OutputAnimTag.cs
 * 
 * OutputAnimTag.cs outputs the animator state info and it's layer off to ArmAnimObserver in order to establish
 * what state the arms are in. Ex, being in a non-interruptable animation.
 *
 */

public class OutputAnimTag : StateMachineBehaviour
{
    public ArmAnimObserver animObserver;

    // updates ArmAnimObserver everytime a animation transitions to a new animation in order to inform aminObserver
    // of our arms being in a new possible state as being in a interruptale animation
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animObserver.UpdateAnimState(stateInfo, layerIndex);
    }
}
