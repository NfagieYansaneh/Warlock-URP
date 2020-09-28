using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//https://stackoverflow.com/questions/1313297/is-there-a-function-type-in-c

public class G36_sEnemy : MonoBehaviour
{
    [Range(0, 180)]
    [Tooltip("Simply, this is for adjusting the field of view concerning the detecting for the player")]
    public float detectionAngle;
    [Range(0, 75)]
    [Tooltip("Detection range for detecting the player")]
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
    public float radius;

    [HideInInspector]
    public NavMeshAgent navMeshAgent;

    public float detectPlayerPeriodFloat;
    private WaitForSeconds detectPlayerPeriod;

    public GameObject[] pooledBullets;
    public TrailRenderer[] pooledTrailRenderers;

    public int poolSize;
    public int pooledBulletsIndex = 0;
    public Transform gunTransform;
    public Transform containerTransform;

    [HideInInspector]
    public GenericEnemyHandler genericEnemyHandler;

    public GameObject bulletPrefab;
    public LayerMask bulletLayerMask;

    public bool detectedPlayer;

    public void Awake()
    {
        genericEnemyHandler = GetComponent<GenericEnemyHandler>();
    }

    public void Start()
    {
        genericEnemyHandler = GetComponent<GenericEnemyHandler>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        genericEnemyHandler.Die = DeathResponse;
        genericEnemyHandler.Hit = HitResponse;
        genericEnemyHandler.Move = MoveResponse;

        detectPlayerPeriod = new WaitForSeconds(detectPlayerPeriodFloat);

        InitBulletPool();
        StartCoroutine("DetectPlayer");
    }

    bool a = true;
    public void Update()
    {
        if (detectedPlayer)
        {
            if (a)
            {
                StartCoroutine("Firing");
                a = false;
            }
            CheckAndMoveBullets();
            transform.LookAt(genericEnemyHandler.playerTransform);
        }
    }

    /* Generic Enemy Handler responses */
    public void HitResponse(int damage)
    {
        genericEnemyHandler.health -= damage;
        if(genericEnemyHandler.health < 0)
        {
            DeathResponse();
        }
    }

    public void DeathResponse()
    {
        genericEnemyHandler.aiOverseer.ClearAiFromRoom(genericEnemyHandler.roomIndex, genericEnemyHandler.aiNumber);
        containerTransform.gameObject.SetActive(false);
    }

    public void MoveResponse(Vector3 position)
    {
        navMeshAgent.SetDestination(position);
    }

    WaitForSeconds x = new WaitForSeconds(0.65f);
    public IEnumerator Firing()
    {
        while (true)
        {
            FireBulletPool();
            yield return x;
        }
    }

    public IEnumerator DetectPlayer()
    {
        Vector3 targetDir;

        while (true)
        {
            // I think i can optimize this into just using Random.value once....

            targetDir = genericEnemyHandler.playerTransform.position - transform.position;

            if (Vector3.Angle(targetDir, transform.forward) <= detectionAngle && targetDir.magnitude <= detectionRange)
            {
                detectedPlayer = true;
                if (Random.value <= requestCoverChance)
                {
                    RequestCover();
                    yield break;
                }

                if (Random.value <= requestChaseChance)
                {
                    detectedPlayer = true;
                    while (true)
                    {
                        RequestChase();
                        yield return detectPlayerPeriod;
                    }
                }

                if (Random.value <= requestDistanceChance)
                {
                    detectedPlayer = true;
                    WaitForSeconds z = new WaitForSeconds(0.85f);

                    while (true)
                    {
                        RequestDistance();
                        yield return z;
                    }
                }

                Debug.LogError("Ai did not react to player being spotted");
            }

            yield return detectPlayerPeriod;
        }
    }

    public void RequestCover()
    {
        navMeshAgent.SetDestination(genericEnemyHandler.aiOverseer.RequestCover(genericEnemyHandler.roomIndex));
    }

    public void RequestChase()
    {
        navMeshAgent.SetDestination(genericEnemyHandler.playerTransform.position);
    }

    public void RequestDistance()
    {
        FireBulletPool();
        navMeshAgent.SetDestination(genericEnemyHandler.aiOverseer.RequestDistance(genericEnemyHandler.roomIndex, genericEnemyHandler.playerTransform.position, radius));
    }

    public void InitBulletPool()
    {
        pooledBullets = new GameObject[poolSize];
        pooledTrailRenderers = new TrailRenderer[poolSize];

        for (int index = 0; index < pooledBullets.Length; index++)
        {
            pooledBullets[index] = Instantiate(bulletPrefab, containerTransform);
            pooledTrailRenderers[index] = pooledBullets[index].GetComponent<TrailRenderer>();
            pooledTrailRenderers[index].emitting = false;
            pooledBullets[index].SetActive(false);
        }
    }

    public void FireBulletPool()
    {
        pooledBullets[pooledBulletsIndex].SetActive(true);
        pooledBullets[pooledBulletsIndex].transform.position = gunTransform.position;
        pooledBullets[pooledBulletsIndex].transform.LookAt(genericEnemyHandler.playerTransform);
        pooledTrailRenderers[pooledBulletsIndex].emitting = true;
        if (pooledBulletsIndex == pooledBullets.Length - 1) pooledBulletsIndex = 0;
        else pooledBulletsIndex++;
    }

    public void CheckAndMoveBullets()
    {
        for (int index = 0; index < pooledBullets.Length; index++)
        {
            if (pooledBullets[index].activeSelf)
            {
                Collider[] colliders = Physics.OverlapSphere(pooledBullets[index].transform.position, 0.35f, bulletLayerMask);

                foreach (Collider c in colliders)
                {
                    pooledTrailRenderers[index].emitting = false;
                    pooledBullets[index].SetActive(false);
                    continue;
                }

                pooledBullets[index].transform.position += pooledBullets[index].transform.forward * 0.65f;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "AiRoomID")
        {
            genericEnemyHandler.roomIndex = other.gameObject.GetComponent<RoomID>().room;
            Debug.Log(genericEnemyHandler.roomIndex);
        }
    }
}
