using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericEnemyHandler : MonoBehaviour
{
    public float health;
    public delegate void genericDelegateFunction ();
    public genericDelegateFunction testCall;
}
