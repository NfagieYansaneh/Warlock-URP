﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Purpose of Kindred_MW.cs
 * 
 * Kindred_MW.cs contains "MW" in its name which means Magic Weapon
 * 
 * Kindred_MW.cs is merely another spell the player can use. But as for now, its merely logs into the debug
 * terminal that is has been casted.
 */

[CreateAssetMenu(fileName = "New Kindred Spell", menuName = "Item/Active/Spell/Kindred")]
public class Kindred_MW : BaseSpell
{
    public override void Use()
    {
        Debug.Log("Used KINDRED spell");
    }

    // below is a very uneffective means of have a 3d cursor for Kindred_MW.cs in-game

    /*
    public float radius;
    public float distance;
    public LayerMask layerMask;
    private GameObject sphere;
    public void OnEnable() {
        sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.localPosition = new Vector3(0.2f, 0.2f, 0.2f);
        sphere.gameObject.SetActive(false);
    }
    public void OnDestroy()
    {
        Destroy(sphere.gameObject);
        DestroyImmediate(sphere.gameObject);
    }
    public void OnDisable()
    {
        Destroy(sphere.gameObject);
        DestroyImmediate(sphere.gameObject);
    }
    public override void AlternativeCrosshairActive()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distance, layerMask))
        {
            sphere.transform.position = hit.point;
            sphere.gameObject.SetActive(true);
        }
        else
        {
            RaycastHit[] hits = Physics.SphereCastAll(ray, radius, distance, layerMask);

            if (hits.Length > 0)
            {
                for (int i = 0; i < hits.Length; i++)
                {
                    if (Vector3.Distance(hits[i].point, sphere.transform.position) <= radius)
                    {
                        Debug.DrawLine(Camera.main.transform.position + (Vector3.up * -0.5f), hits[0].point, Color.red, 0.5f);
                        sphere.gameObject.SetActive(true);
                    }
                }
            }
            else
            {
                sphere.gameObject.SetActive(false);
            }
        }
    }
    */
}
