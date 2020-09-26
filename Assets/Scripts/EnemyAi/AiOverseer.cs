using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;

public class AiOverseer : MonoBehaviour
{
    public GameObject[] pooledAiObjects;
    public RoomID[] roomIDs;
    public Transform playerTransform;
    public GameObject test;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //NavMeshBuilder.BuildNavMeshData();
    }
    //Random.insideUnitSphere

    public int AppendFormerRogueAiToRoom(Rooms roomIndex, GenericEnemyHandler genericEnemyHandler)
    {
        for(int i=0; i< roomIDs[(int)roomIndex].genericEnemyHandlers.Length; i++) { 
            if(roomIDs[(int)roomIndex].genericEnemyHandlers[i] == null) { 
                roomIDs[(int)roomIndex].genericEnemyHandlers[i] = genericEnemyHandler;
                return i;
            }
        }

        return -1;
    }

    /* Debug Ai Overseer Powers */
    public void SpawnRandomlyInRoom(Rooms roomIndex)
    {
        if (!roomIDs[(int)roomIndex].full) { 
            float rndX = Random.Range(0f, roomIDs[(int)roomIndex].width);
            float rndZ = Random.Range(0f, roomIDs[(int)roomIndex].length);

            Vector3 position = new Vector3(rndX + roomIDs[(int)roomIndex].transform.position.x, 0f, rndZ + roomIDs[(int)roomIndex].transform.position.z);
            GameObject obj = Instantiate(pooledAiObjects[0], position, Quaternion.identity);

            GenericEnemyHandler handler = obj.GetComponentInChildren<GenericEnemyHandler>();
            handler.roomIndex = roomIndex;
            handler.aiOverseer = GetComponent<AiOverseer>();
            handler.playerTransform = playerTransform;

            if(roomIDs[(int)roomIndex].genericEnemyHandlers[roomIDs[(int)roomIndex].genericEnemyHandlers.Length - 1] != null)
            {
                roomIDs[(int)roomIndex].full = true;
            }
        }

    }
    public void KillAllInLobby()
    {
        for (int i = 0; i < roomIDs[(int)Rooms.Lobby].genericEnemyHandlers.Length; i++)
        {
            if(roomIDs[(int)Rooms.Lobby].genericEnemyHandlers[i] != null) { 
                Debug.Log("Killied Enemy {"+ roomIDs[(int)Rooms.Lobby].genericEnemyHandlers[i].aiNumber +"} in Lobby");
                roomIDs[(int)Rooms.Lobby].genericEnemyHandlers[i].Die();
                roomIDs[(int)Rooms.Lobby].genericEnemyHandlers[i] = null;
                roomIDs[(int)Rooms.Lobby].full = false;
            }
        }
    }

    public Vector3 RequestCover(Rooms roomIndex)
    {

        for (int x=0; x<roomIDs[(int)roomIndex].fullCoverTransforms.Length; x++)
        {
            if (roomIDs[(int)roomIndex].fullCoverAvailable[x])
            {
                roomIDs[(int)roomIndex].fullCoverAvailable[x] = false;
                return roomIDs[(int)roomIndex].fullCoverTransforms[x].position;
            }
        }

        for (int x = 0; x < roomIDs[(int)roomIndex].halfCoverTransforms.Length; x++)
        {
            if (roomIDs[(int)roomIndex].halfCoverAvailable[x])
            {
                roomIDs[(int)roomIndex].halfCoverAvailable[x] = false;
                return roomIDs[(int)roomIndex].halfCoverTransforms[x].position;
            }
        }

        return Vector3.zero;
    }

    public Vector3 RequestDistance(Rooms roomIndex, Vector3 otherPosition, float radius)
    {
        Debug.Log("Requested Distance");
        //2*radius
        float rndX = Random.Range(0f, roomIDs[(int)roomIndex].width);
        float rndZ = Random.Range(0f, roomIDs[(int)roomIndex].length);

        float posX = rndX + roomIDs[(int)roomIndex].topLeft.x - roomIDs[(int)roomIndex].width;
        float posZ = rndZ + roomIDs[(int)roomIndex].topLeft.z;

        float a = playerTransform.position.z - roomIDs[(int)roomIndex].topLeft.z - radius;
        float b = playerTransform.position.x + roomIDs[(int)roomIndex].topLeft.x - radius;

        if(rndZ > Mathf.Abs(a) && rndZ < Mathf.Abs(a+2*radius)
            && rndX > Mathf.Abs(b) && rndX < Mathf.Abs(b+2*radius))
        {
            posZ += -a + (playerTransform.position.z + radius - posZ);
            posX += -b + (playerTransform.position.x + radius - posX);
        }

        Vector3 dest = new Vector3(posX, 0f, posZ);

        //Vector3 testVector = new Vector3(playerTransform.position.x - roomIDs[(int)roomIndex].topLeft.x - radius, 0f, playerTransform.position.z - roomIDs[(int)roomIndex].topLeft.z - radius);
        Instantiate(test, dest, Quaternion.identity);
        Debug.Log(dest);
        return dest;
    }
}
