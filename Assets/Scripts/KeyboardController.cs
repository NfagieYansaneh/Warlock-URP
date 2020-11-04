using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/* Purpose of KeyboardController.cs
 * 
 * Every frame, PlayerController.cs calls GetKeys(); in order to obtain user input
 * I handle the acquistion of keyboard keys into just one file in order to make sure
 * I never call Input.GetKey for a specific key more than once. Ultimately aiding preformance and
 * preventing obfuscated code
 */

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

        public bool keyF;
        public bool keyDownF;

        public float mouseX;
        public float mouseY;
        
        public bool mouse1;
        public bool mouse2;
        public bool mouse3;
        public float mouseScroll;

        public bool keyDownC;
        public bool keyUpC;
        public bool spacebar;
        public bool keyDownSpacebar;
        //public int moveJmps;

        public bool[] spellKeys;

        public void GetKeys()
        {
            keyA = Convert.ToInt32(Input.GetKey(KeyCode.A)); // A
            keyD = Convert.ToInt32(Input.GetKey(KeyCode.D)); // D
            keyW = Convert.ToInt32(Input.GetKey(KeyCode.W)); // W
            keyS = Convert.ToInt32(Input.GetKey(KeyCode.S)); // S

            keyF = Input.GetKey(KeyCode.F); // F

            keyR = Input.GetKeyDown(KeyCode.R); // R

            mouseX = Input.GetAxis("Mouse X"); // horizontal mouse movement
            mouseY = Input.GetAxis("Mouse Y"); // vertical mouse movement

            mouse1 = Input.GetMouseButtonDown(0); // left click
            mouse2 = Input.GetMouseButtonDown(1); // right click
            mouse3 = Input.GetMouseButtonDown(2); // middle mouse button

            mouseScroll = Input.GetAxis("Mouse ScrollWheel"); // scroll wheel

            spellKeys[0] = Input.GetKeyDown(KeyCode.Alpha1); // 1
            spellKeys[1] = Input.GetKeyDown(KeyCode.Alpha2); // 2
            spellKeys[2] = Input.GetKeyDown(KeyCode.Alpha3); // 3
            spellKeys[3] = Input.GetKeyDown(KeyCode.Alpha4); // 4

            keyDownF = Input.GetKeyDown(KeyCode.F); // F

            keyDownC = Input.GetKeyDown(KeyCode.C); // key down C
            keyUpC = Input.GetKeyUp(KeyCode.C); // key up C

            spacebar = Input.GetKey(KeyCode.Space); // spacebar held
            keyDownSpacebar = Input.GetKeyDown(KeyCode.Space); // key down spacebar
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
