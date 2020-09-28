using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GenericEnemyHandler : MonoBehaviour
{
    public float health;
    public Rooms roomIndex;
    [HideInInspector]
    public int aiNumber;
    public AiOverseer aiOverseer;
    public Transform playerTransform;
    public NavMeshAgent navMeshAgent;
    
    private void Start()
    {
        // "this" doesnt work
        navMeshAgent = GetComponent<NavMeshAgent>();
        aiOverseer.AppendAiToRoom(roomIndex, GetComponent<GenericEnemyHandler>());
    }

    public delegate void deathDelegateFunction ();
    public deathDelegateFunction Die;

    public delegate void hitDelegateFunction(int damage);
    public hitDelegateFunction Hit;

    public delegate void moveDelegateFunction(Vector3 position);
    public moveDelegateFunction Move;
}
