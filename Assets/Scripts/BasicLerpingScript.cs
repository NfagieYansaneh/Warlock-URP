using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Purpose of BasicLerpingScript.cs
 * 
 * BasicLerpingScript.cs was merely made for quick debugging as I was testing how my Rainbow
 * Decals appeared on moving objects
 */

public class BasicLerpingScript : MonoBehaviour
{
    public float magnitude;
    public Vector3 direction;
    public float time;

    private Vector3 dest;

    void Update()
    {
        // object swings back and forth because of the nature of sinusodial functions
        dest = (magnitude * Mathf.Sin(Time.time)) * direction;
        transform.position = Vector3.Lerp(transform.position, dest + transform.position, time);
    }
}
