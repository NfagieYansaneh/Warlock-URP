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

    public bool inCover;
    public Transform coverPosition;

    public float detectPlayerPeriodFloat;
    private WaitForSeconds detectPlayerPeriod;

    public GameObject[] pooledBullets;
    public GenericBulletData[] pooledBulletsData;
    public TrailRenderer[] pooledTrailRenderers;

    public int poolSize;
    public int pooledBulletsIndex = 0;
    public int bulletDamage; // use this instead of hard coding in values
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

        genericEnemyHandler.Die = DeathResponse;
        genericEnemyHandler.Hit = HitResponse;
        genericEnemyHandler.Move = MoveResponse;
        genericEnemyHandler.Chase = RequestChase;

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
        genericEnemyHandler.aiOverseer.playerHealthController.IncreaseGreyHealth(12f); // dont hardcode this, change this later
        genericEnemyHandler.aiOverseer.playerHealthController.IncreaseScore(24f);
        genericEnemyHandler.aiOverseer.ClearAiFromRoom(genericEnemyHandler.roomIndex, genericEnemyHandler.aiNumber);
        containerTransform.gameObject.SetActive(false);
    }

    public void MoveResponse(Vector3 position)
    {
        genericEnemyHandler.navMeshAgent.SetDestination(position);
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
                    inCover = true;
                    StartCoroutine("Cover_CheckIfExposed");
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

    public IEnumerator Cover_CheckIfExposed()
    {
        while (true)
        {
            Vector3 targetDir = genericEnemyHandler.playerTransform.position - coverPosition.position;
            Debug.DrawRay(coverPosition.position, coverPosition.forward, Color.red, 1f);
            if (Vector3.Angle(targetDir, coverPosition.forward) <= 70f)
            {
                Vector3 scatterPosition = coverPosition.position;
                scatterPosition += coverPosition.forward * 1f; // distance perpendicular to cover position
                scatterPosition += -targetDir.normalized * 1f; // distance away from player
                //scatterPosition.y = 0f;
                genericEnemyHandler.navMeshAgent.SetDestination(scatterPosition);
                yield break;
            }

            yield return x;
        }
    }

    public void RequestCover()
    {
        coverPosition = genericEnemyHandler.aiOverseer.RequestCover(genericEnemyHandler.roomIndex);
        genericEnemyHandler.navMeshAgent.SetDestination(coverPosition.position);
    }

    public void RequestChase() // improper naming since we aren't talking to the aiOverseer
    {
        genericEnemyHandler.navMeshAgent.SetDestination(genericEnemyHandler.playerTransform.position);
    }

    public void RequestDistance()
    {
        FireBulletPool();
        genericEnemyHandler.navMeshAgent.SetDestination(genericEnemyHandler.aiOverseer.RequestDistance(genericEnemyHandler.roomIndex, genericEnemyHandler.playerTransform.position, radius));
    }

    public void InitBulletPool()
    {
        pooledBullets = new GameObject[poolSize];
        pooledBulletsData = new GenericBulletData[poolSize];
        pooledTrailRenderers = new TrailRenderer[poolSize];

        for (int index = 0; index < pooledBullets.Length; index++)
        {
            pooledBullets[index] = Instantiate(bulletPrefab, containerTransform);
            pooledBulletsData[index] = pooledBullets[index].gameObject.GetComponent<GenericBulletData>();
            pooledBulletsData[index].damage = 8;
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
                    if (c.CompareTag("Player")) genericEnemyHandler.aiOverseer.playerHealthController.TakeDamage(pooledBulletsData[index].damage);

                    pooledTrailRenderers[index].emitting = false;
                    pooledBullets[index].SetActive(false);
                    continue;
                }

                pooledBullets[index].transform.position += pooledBullets[index].transform.forward * 0.65f;
            }
        }
    }

    private void OnTriggerEnter(Collider other) // re work this
    {
        if (other.gameObject.tag == "AiRoomID")
        {
            genericEnemyHandler.roomIndex = other.gameObject.GetComponent<RoomID>().room;
            Debug.Log(genericEnemyHandler.roomIndex);
        }
    }
}
