using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum AiOverseerQueuedCommands { Chase, Distance, Etc }; // these are queued commands trigger when we enter a desired room
public class GenericEnemyHandler : MonoBehaviour
{
    public float health;
    public Rooms roomIndex;
    [HideInInspector]
    public int aiNumber;
    public AiOverseer aiOverseer;
    public Transform playerTransform;
    public NavMeshAgent navMeshAgent;

    public bool overseerQueuedCommand;
    public Rooms queuedRoom;
    public Rooms toRoomQueued;
    public AiOverseerQueuedCommands command;

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

    public delegate void chaseDelegateFunction();
    public chaseDelegateFunction Chase;

    // We need a function to reassign units to another room which will be really easy to implement
    // We need a function to reassign units to another room which will be really easy to implement
    // We need a function to reassign units to another room which will be really easy to implement
    // We need a function to reassign units to another room which will be really easy to implement
    // We need a function to reassign units to another room which will be really easy to implement

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered");
        if (overseerQueuedCommand)
        {
            RoomID roomID = other.gameObject.GetComponent<RoomID>();
            if (roomID == null || roomID.room != queuedRoom) return;

            switch (command)
            {
                case AiOverseerQueuedCommands.Distance:
                    int layerMask = ~navMeshAgent.areaMask;
                    navMeshAgent.areaMask |= layerMask;
                    Move(aiOverseer.RequestDistance(toRoomQueued, Vector3.zero, 0f));   // We need a function to reassign units to another room which will be really easy to implement
                    overseerQueuedCommand = false;
                    break;

                default:
                    break;
            }
        }
    }
}
