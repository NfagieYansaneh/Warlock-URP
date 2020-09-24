using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericEnemyHandler : MonoBehaviour
{
    public float health;

    public delegate void deathDelegateFunction ();
    public deathDelegateFunction Die;

    public delegate void hitDelegateFunction(int damage);
    public hitDelegateFunction Hit;

    public delegate void moveDelegateFunction();
    public moveDelegateFunction Move;
}
