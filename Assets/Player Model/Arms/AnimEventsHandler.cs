using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEventsHandler : MonoBehaviour
{
    [Tooltip("Allows communication to an InventoryObject")]
    public InventoryObject inventory;
    public UiManager uiManager;
    BaseSpell curSpell;

    //BaseGun curGun;
    public Transform L_wristBone;
    public ArmAnimObserver armAnimObserver;

    public LineRenderer lineRenderer;
    [Tooltip("Obtains camera camera controller for screen shakes")]
    public CameraController cameraController;

    [Header("Debug : Visualizing spell casts")]
    [Range(10f, 1000f)]
    public float range;
    public Camera mainCamera;

    public void Start()
    {
        curSpell = inventory.nullSpell;
        //curGun = inventory.nullGun;

        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false;
    }

    public void FireGun ()
    {
        //Debug.LogWarning("Called");
        inventory.guns[inventory.curGunIndex].ammoInMag -= 1;
        inventory.guns[inventory.curGunIndex].FireRaycast();
        uiManager.UpdateGunDisplay();
    }

    public void ReloadGun ()
    {
        inventory.guns[inventory.curGunIndex].ammo += inventory.guns[inventory.curGunIndex].ammoInMag - inventory.guns[inventory.curGunIndex].maxAmmoInMag;
        inventory.guns[inventory.curGunIndex].ammoInMag = inventory.guns[inventory.curGunIndex].maxAmmoInMag;
        uiManager.UpdateGunDisplay();
    }

    public void SoloHandEvent ()
    {
        curSpell.Use();
        Debug.Log(curSpell.description);

        //Debugging
        DefineDebugLine(curSpell); // Defines debug line for visualizing spell casts
        StartCoroutine("DrawDebugLine"); // Draws debug line for visualizing spell casts*/
    }

    public void DoubleHandEvent ()
    {
        curSpell.Use();
    }

    public void SetSpell(BaseSpell spell)
    {
        curSpell = spell;
    }

    public void TransitionLeftAnimStateTo(int newAnimState)
    {
        int a = armAnimObserver.animState[0];
        armAnimObserver.animState[0] = newAnimState;
        armAnimObserver.updateDebugTextIfEnabled();
        Debug.Log("Transitioned '" + a + "' to '" + armAnimObserver.animState[0] + "'");
    }

    public void TransitionRightAnimStateTo(int newAnimState)
    {
        int a = armAnimObserver.animState[1];
        armAnimObserver.animState[1] = newAnimState;
        armAnimObserver.updateDebugTextIfEnabled();
        Debug.Log("Transitioned '" + a + "' to '" + armAnimObserver.animState[1] + "'");
    }

    public void TransitionBothAnimStateTo(int newAnimState)
    {

    }

    //Debugging
    public void DefineDebugLine(BaseSpell spell)
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        lineRenderer.SetPosition(0, L_wristBone.position + (L_wristBone.right * 0.25f) + (L_wristBone.up * 0.5f));
        lineRenderer.SetPosition(1, mainCamera.transform.position + (ray.direction * range));
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
