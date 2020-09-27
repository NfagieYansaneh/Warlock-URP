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

    public void AppendAiToRoom(Rooms roomIndex, GenericEnemyHandler genericEnemyHandler)
    {
        Debug.Log(roomIndex);
        for(int i=0; i< roomIDs[(int)roomIndex].genericEnemyHandlers.Length; i++) {
            if(roomIDs[(int)roomIndex].genericEnemyHandlers[i] == null) { // using null to check just doesn't work at all like wtf
                roomIDs[(int)roomIndex].genericEnemyHandlers[i] = genericEnemyHandler;
                genericEnemyHandler.aiNumber = i;
                genericEnemyHandler.roomIndex = roomIndex;
                Debug.LogError(roomIndex + " : " + genericEnemyHandler.aiNumber + " : " + roomIDs[(int)roomIndex].genericEnemyHandlers[i]);
                break;
            }
        }
    }

    public void ClearAiFromRoom(Rooms roomIndex, GenericEnemyHandler genericEnemyHandler)
    {
        roomIDs[(int)roomIndex].genericEnemyHandlers[genericEnemyHandler.aiNumber] = null;
    }

    /* Debug Ai Overseer Powers */
    public void KillAllInRoom(Rooms roomIndex)
    {
        for (int i = 0; i < roomIDs[(int)roomIndex].genericEnemyHandlers.Length; i++)
        {
            if (roomIDs[(int)roomIndex].genericEnemyHandlers[i] != null)
            {
                Debug.Log("Killied Enemy {" + roomIDs[(int)roomIndex].genericEnemyHandlers[i].aiNumber + "} in Lobby");
                roomIDs[(int)roomIndex].genericEnemyHandlers[i].Die();
                roomIDs[(int)roomIndex].genericEnemyHandlers[i] = null;
                roomIDs[(int)roomIndex].full = false;
            }
        }
    }

    /* Prototype Functions */
    public void SpawnRandomlyInRoom(Rooms roomIndex)
    {
        if (!roomIDs[(int)roomIndex].full) {
            float rndX = Random.Range(0f, roomIDs[(int)roomIndex].width);
            float rndZ = Random.Range(0f, roomIDs[(int)roomIndex].length);

            float posX = rndX + roomIDs[(int)roomIndex].topLeft.x - roomIDs[(int)roomIndex].width;
            float posZ = rndZ + roomIDs[(int)roomIndex].topLeft.z;

            Vector3 position = new Vector3(posX, 0f, posZ);
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

    public int MoveAllAiToRoom(Rooms fromRoom, Rooms toRoom) // returns the number of ai moved to the other room
    {
        if (roomIDs[(int)toRoom].full) return -1;
        int numberMoved = 0;

        for (int i = 0; i < roomIDs[(int)fromRoom].genericEnemyHandlers.Length; i++)
        {
            if(roomIDs[(int)fromRoom].genericEnemyHandlers[i] != null)
            {
                Vector3 position = RequestDistance(toRoom, Vector3.zero, 0f);
                roomIDs[(int)fromRoom].genericEnemyHandlers[i].Move(position);

                AppendAiToRoom(toRoom, roomIDs[(int)fromRoom].genericEnemyHandlers[i]);
                ClearAiFromRoom(fromRoom, roomIDs[(int)fromRoom].genericEnemyHandlers[i]);
                numberMoved++;

                if (roomIDs[(int)toRoom].full) break;
            }
        }

        return numberMoved;
    }

    /* Definitive Functions */

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

        if (otherPosition != Vector3.zero && radius != 0f) { 
            float a = playerTransform.position.z - roomIDs[(int)roomIndex].topLeft.z - radius;
            float b = playerTransform.position.x + roomIDs[(int)roomIndex].topLeft.x - radius;

            if(rndZ > Mathf.Abs(a) && rndZ < Mathf.Abs(a+2*radius)
                && rndX > Mathf.Abs(b) && rndX < Mathf.Abs(b+2*radius))
            {
                posZ += -a + (playerTransform.position.z + radius - posZ);
                posX += -b + (playerTransform.position.x + radius - posX);
            }
        }

        Vector3 dest = new Vector3(posX, 0f, posZ);
        return dest;
    }
}
