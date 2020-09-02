using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicLerpingScript : MonoBehaviour
{
    public float magnitude;
    public Vector3 direction;
    public float time;

    private Vector3 dest;

    void Update()
    {
        dest = (magnitude * Mathf.Sin(Time.time)) * direction;
        transform.position = Vector3.Lerp(transform.position, dest + transform.position, time);
    }
}
