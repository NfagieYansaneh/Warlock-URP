using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmComboParser : StateMachineBehaviour
{
    public ArmAnimObserver animObserver = null;
    private PlayerAnimController AnimController = null;

    //static bool latch = false;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(AnimController == null)
        {
            AnimController = animator.gameObject.GetComponent<ArmControllerLinker>().AnimController;
        }
        //latch = !latch;

        animObserver.UpdateAnimState(stateInfo, layerIndex);
        AnimController.SetParameter((int)ArmAnimParams.CastingAnimIndex, AnimController.animations[AnimController.pointer], (int)ArmAnimLayer.allLayers, (int)ArmAnimState.isAny);
        //Debug.Log(AnimController.pointer);
        AnimController.pointer++;
    }
}
