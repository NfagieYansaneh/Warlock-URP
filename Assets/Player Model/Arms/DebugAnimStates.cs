using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* Purpose of DebugAnimStates.cs
 * 
 * DebugAnimStates.cs is purely for when I was debugging which states my arms we in during an animation. Upon calling
 * updateText(); we end up stating what states are arms are in during animation in the top left corner Ui element by
 * merely re-writing its stored text Ui object. No scripts are dependent on DebugAnimState.cs, so disabling it in-game
 * will have no consequence
 * 
 * DebugAnimStates.cs communicates with ArmAnimObserver in order to acquire knowledge of what states are arms are in during animation
 */

public class DebugAnimStates : MonoBehaviour
{
    public Text debugTextObj; // text Ui element that we re-write in order to display our debug data on the player's screen
    public ArmAnimObserver armAnimObserver;
    public bool debugObservations; // A way to stop DebugAnimState.cs from further updating text Ui element with debug data on what states the arms are in during animation
    
    private string[] states = { "is idle", "is in action", "is in an interruptable action", "is in a non-interruptable action" };

    private void Start()
    {
        // defines ArmAnimObserver's delegate function so ArmAnimObserver can call that function when it is appropiate to
        // call updateText(); in order to save computing power
        armAnimObserver.updateDebugTextIfEnabled = updateText;
    }

    public void updateText()
    {
        if (debugObservations) { 
            string leftArmState = states[armAnimObserver.animState[0]];
            string rightArmState = states[armAnimObserver.animState[1]];

            debugTextObj.text = "Left Arm : " + leftArmState + "\nRight Arm : " + rightArmState + "\nTime @ Update : " + Time.time;
        }
    }
}
