using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DebugAnimStates : MonoBehaviour
{
    public Text debugTextObj;
    public ArmAnimObserver armAnimObserver;
    public bool debugObservations;
    
    private string[] states = { "is idle", "is in action", "is firing gun", "is in an interruptable action" };

    private void Start()
    {
        armAnimObserver.updateDebugTextIfEnabled = updateText;
    }

    public void updateText()
    {
        if (debugObservations) { 
            string a = states[armAnimObserver.animState[0]];
            string b = states[armAnimObserver.animState[1]];

            debugTextObj.text = "Left Arm : " + a + "\nRight Arm : " + b + "\nTime @ Update : " + Time.time;
        }
    }
}
