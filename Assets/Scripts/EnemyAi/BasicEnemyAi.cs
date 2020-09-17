using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicEnemyAi : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public Transform playerTransform;
    [Range(-1, 1)]
    public float detectionAngle;
    [Range(1, 10)]
    public float detectionRange;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        StartCoroutine("detectPlayer");
        //navMeshAgent.SetDestination( new Vector3(0, 0, 0));
    }

    WaitForSeconds x = new WaitForSeconds(0.3f);

    IEnumerator detectPlayer()
    {
        while (true)
        {
            Debug.Log(Vector3.Dot(transform.position, playerTransform.position));
            if (Vector3.Dot(transform.position.normalized, playerTransform.position.normalized) >= detectionAngle && Vector3.Distance(transform.position, playerTransform.position) <= detectionRange)
            {
                transform.LookAt(playerTransform);
                Debug.DrawLine(transform.position, playerTransform.position, Color.white);
                Debug.LogError("PLAYER DETECTED!!!");
            }

            yield return x;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
