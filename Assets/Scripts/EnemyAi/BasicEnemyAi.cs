using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicEnemyAi : MonoBehaviour
{
    // Use scriptable objects to make this simplier

    public NavMeshAgent navMeshAgent;
    public GameObject navMeshObstacleBubble;
    public Transform gunTransform;
    public Transform containerTransform;

    public Transform playerTransform;
    [Range(0, 180)]
    public float detectionAngle;
    [Range(0, 75)]
    public float detectionRange;
    [Range(0, 1)]
    [Tooltip("'0' Means No chance to request cover, while '1' Means Full chance to request cover")]
    public float requestCoverChance;
    [Range(0, 1)]
    [Tooltip("'0' Means No chance to request chase, while '1' Means Full chance to request chase")]
    public float requestChaseChance;
    [Range(0, 1)]
    [Tooltip("'0' Means No chance to request distance, while '1' Means Full chance to request distance")]
    public float requestDistanceChance;
    [Range(0, 5)]
    [Tooltip("Distancing Bubble Radius")]
    public float bubbleRadius;

    public Rooms roomIndex;
    public AiOverseer aiOverseer;

    public GameObject bulletPrefab;
    [HideInInspector]
    public GameObject[] pooledBullets = new GameObject[20];
    public TrailRenderer[] pooledTrailRenderers = new TrailRenderer[20];


    private bool detectedPlayer = false;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        InitBulletPool();
        StartCoroutine("detectPlayer");

        //navMeshAgent.SetDestination( new Vector3(0, 0, 0));
    }

    private Vector3 targetDir;
    private WaitForSeconds x = new WaitForSeconds(0.15f);

    IEnumerator detectPlayer()
    {
        while (true)
        {
            // I think i can optimize this into just using Random.value once....

            targetDir = playerTransform.position - transform.position;

            if (Vector3.Angle(targetDir, transform.forward) <= detectionAngle && targetDir.magnitude <= detectionRange)
            {
                if(Random.value <= requestCoverChance) {
                    detectedPlayer = true;
                    navMeshAgent.SetDestination(aiOverseer.RequestCover(roomIndex));
                    yield break;
                }

                if(Random.value <= requestChaseChance)
                {
                    while (true)
                    {
                        detectedPlayer = true;
                        //Debug.Log("REQUESTED CHASE!!!");
                        navMeshAgent.SetDestination(playerTransform.position);
                        yield return x;
                    }
                }

                if (Random.value <= requestDistanceChance)
                {
                    detectedPlayer = true;
                    WaitForSeconds z = new WaitForSeconds(0.55f);
                    while (true) { 
                        navMeshAgent.SetDestination(aiOverseer.RequestDistance(roomIndex, playerTransform.position, 2 * 3f));
                        FireBulletPool();

                        yield return z;
                    }
                    //navMeshAgent.SetDestination(playerTransform.position);
                }

                detectedPlayer = true;
                //Debug.LogError("PLAYER DETECTED!!!");
            }

            yield return x;
        }
    }

    void Update()
    {
        if (detectedPlayer)
        {
            transform.LookAt(playerTransform);
            for (int index = 0; index < pooledBullets.Length; index++)
            {
                if (pooledBullets[index].activeSelf)
                {
                    pooledBullets[index].transform.position += pooledBullets[index].transform.forward * 0.2f;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "AiRoomID")
        {
            roomIndex = other.gameObject.GetComponent<RoomID>().room;
            Debug.Log(roomIndex);
        }
    }

    void InitBulletPool()
    {
        for (int index=0; index<pooledBullets.Length; index++)
        {
            pooledBullets[index] = Instantiate(bulletPrefab, containerTransform);
            pooledTrailRenderers[index] = pooledBullets[index].GetComponent<TrailRenderer>();
            pooledTrailRenderers[index].emitting = false;
            pooledBullets[index].SetActive(false);
        }
    }

    void FireBulletPool()
    {
        for (int index=0; index<pooledBullets.Length; index++)
        {
            if (!pooledBullets[index].activeSelf) { 
                pooledBullets[index].transform.position = gunTransform.position;
                pooledBullets[index].transform.rotation = gunTransform.rotation;
                pooledTrailRenderers[index].emitting = true;
                pooledBullets[index].SetActive(true);
                break;
            }
        }
    }
}
