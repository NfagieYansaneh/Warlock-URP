using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Purpose of AnimEventsHandler.cs
 * 
 * AnimEventsHandler.cs handles events that occur during animations, because during animations, at specific points, we want
 * guns to be fired, spells be to activates, or visual effects to be shown. AnimEventsHandler.cs stores all these event functions
 * here and Unity allows me to timestamp my animation to call said functions, stored within AnimEventsHandler.cs, at any specific
 * and desired time
 * 
 * AnimEventsHandler.cs ...
 * Communicates with InventoryObject in order to trigger cooldowns or deplete ammo of the corressponding spells or guns during animations at specific times
 * Communicates with UiManager in order to promptly update the corressponding Ui Elements during an animation event
 * Communicates with LineRenderer in order to display a debugging line for spells (as seen for our igni spell) when activated, to demonstrate animation events work for spells
 * Communicates with the player's main camera in order to draw these debugging lines (for LineRenderer) properly
 * Communicates with ArmAnimObserver for adding the ability to use animation events to transition the state of a active animation (such as a interruptable active animation transitioning to non-interruptable)
 * 
 */

public class AnimEventsHandler : MonoBehaviour
{
    [Tooltip("Allows communication to an InventoryObject")]
    public InventoryObject inventory;
    public UiManager uiManager;

    // by setting the current spell before we have an animation use an animation event, we can prevent an obfustacted method of checking every spell in order to see which one was activated
    BaseSpell curSpell;

    //BaseGun curGun;
    public Transform L_wristBone; // used so we can properly position the debugging line at the player's left palm

    // used so we can use animation events to transition the state of arms. Ex, transition an arm's animation mid-way from non-interruptable to interruptable
    public ArmAnimObserver armAnimObserver;

    public LineRenderer lineRenderer;
    [Tooltip("Obtains camera camera controller for screen shakes")]
    public CameraController cameraController;

    [Header("Debug : Visualizing spell casts")]
    [Range(10f, 1000f)]
    public float range; // range of debugging line
    public Camera mainCamera;

    public void Start()
    {
        curSpell = inventory.nullSpell;
        //curGun = inventory.nullGun;

        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false;
    }

    // basic firing gun animation event in which we deplete the current gun's ammo by 1, and fire a bullet from its bullet pool whilst updating the gun Ui elements
    // such as ammo
    public void FireGun ()
    {
        //Debug.LogWarning("Called");
        inventory.guns[inventory.curGunIndex].ammoInMag -= 1;
        inventory.guns[inventory.curGunIndex].FireBulletFromPool();
        uiManager.UpdateGunDisplay();
    }
    
    // basic reloading animation event in which we set the current's gun ammo in mag to it's max ammo in mag whilst updating the gun Ui elements
    // this process ultimately "reloads" the gun
    public void ReloadGun ()
    {
        inventory.guns[inventory.curGunIndex].ammoInMag = inventory.guns[inventory.curGunIndex].maxAmmoInMag;
        uiManager.UpdateGunDisplay();
    }

    // SoloHandEvent is an animation event for single hand spells and activating their effects/abilities by merely calling the Use(); of our
    // current spell which is to all ways be defined before a spell is used by PlayerSpellController.cs or any another script for that matter
    public void SoloHandEvent ()
    {
        curSpell.Use();
        //Debug.Log(curSpell.basicDescription);

        //Debugging
        DefineDebugLine(curSpell); // Defines debug line for visualizing spell casts
        StartCoroutine("DrawDebugLine"); // Draws debug line for visualizing spell casts*/
    }

    // DoubleHandEvent is an animation event for double hand spells and activating their effects/abilities by merely calling the Use(); of our
    // current spell which is to all ways be defined before a spell is used by PlayerSpellController.cs or any another script for that matter
    public void DoubleHandEvent ()
    {
        curSpell.Use();
    }

    // Sets our current spell (not an animation event)
    public void SetSpell(BaseSpell spell)
    {
        curSpell = spell;
    }

    // animation event that transitions are left arm into a new animation state (ex, interruptable or idle)
    public void TransitionLeftAnimStateTo(int newAnimState)
    {
        int oldState = armAnimObserver.animState[0];
        armAnimObserver.animState[0] = newAnimState;
        armAnimObserver.updateDebugTextIfEnabled();
        Debug.Log("Transitioned '" + oldState + "' to '" + armAnimObserver.animState[0] + "'");
    }

    // animation event that transitions are right arm into a new animation state (ex, interruptable or idle)
    public void TransitionRightAnimStateTo(int newAnimState)
    {
        int oldState = armAnimObserver.animState[1];
        armAnimObserver.animState[1] = newAnimState;
        armAnimObserver.updateDebugTextIfEnabled();
        Debug.Log("Transitioned '" + oldState + "' to '" + armAnimObserver.animState[1] + "'");
    }

    public void TransitionBothAnimStateTo(int newAnimState)
    {
        // As of now, no arm animation events calls this function and we may never need this function...
    }

    //Debugging
    public void DefineDebugLine(BaseSpell spell)
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        lineRenderer.SetPosition(0, L_wristBone.position + (L_wristBone.right * 0.25f) + (L_wristBone.up * 0.5f)); // start position (origin)
        lineRenderer.SetPosition(1, mainCamera.transform.position + (ray.direction * range)); // end position (destination)
        lineRenderer.material.color = spell.color;
        lineRenderer.startWidth = 0.45f;
        lineRenderer.endWidth = 0.2f;
    }
    WaitForSeconds x = new WaitForSeconds(0.1f);
    IEnumerator DrawDebugLine()
    {
        for (int i = 0; i < 1; i++)
        {
            lineRenderer.enabled = true;
            yield return x;
        }

        lineRenderer.enabled = false;
    }
}
