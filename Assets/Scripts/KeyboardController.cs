using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(fileName="New Keyboard Controller", menuName="Systems/Keyboard Controller")]
public class KeyboardController : ScriptableObject
{
    // Structure containing all key preses & a function to GetKeys()
    public struct Keyboard
    {
        public int keyA;
        public int keyD;
        public int keyW;
        public int keyS;
        public bool keyR;

        public float mouseX;
        public float mouseY;
        
        public bool mouse1;
        public bool mouse2;
        public bool mouse3;
        public float mouseScroll;

        public bool keyDownCtrl;
        public bool keyUpCtrl;
        public bool spacebar;
        public bool keyDownSpacebar;
        //public int moveJmps;

        public bool[] spellKeys;

        public void GetKeys()
        {
            keyA = Convert.ToInt32(Input.GetKey(KeyCode.A));
            keyD = Convert.ToInt32(Input.GetKey(KeyCode.D));
            keyW = Convert.ToInt32(Input.GetKey(KeyCode.W));
            keyS = Convert.ToInt32(Input.GetKey(KeyCode.S));
            keyR = Input.GetKeyDown(KeyCode.R);

            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y");

            mouse1 = Input.GetMouseButtonDown(0);
            mouse2 = Input.GetMouseButtonDown(1);
            mouse3 = Input.GetMouseButtonDown(2);

            mouseScroll = Input.GetAxis("Mouse ScrollWheel");

            spellKeys[0] = Input.GetKeyDown(KeyCode.Alpha1);
            spellKeys[1] = Input.GetKeyDown(KeyCode.Alpha2);
            spellKeys[2] = Input.GetKeyDown(KeyCode.Alpha3);
            spellKeys[3] = Input.GetKeyDown(KeyCode.Alpha4);

            keyDownCtrl = Input.GetKeyDown(KeyCode.C);
            keyUpCtrl = Input.GetKeyUp(KeyCode.C);

            spacebar = Input.GetKey(KeyCode.Space);
            keyDownSpacebar = Input.GetKeyDown(KeyCode.Space);
        }
    }
    public Keyboard Keys = new Keyboard();

    public void OnEnable()
    {
        Keys.spellKeys = new bool[4];
    }
    
    // Reassign keys
    public void Reassign()
    {
        // Reassigns Keys
    }
}
