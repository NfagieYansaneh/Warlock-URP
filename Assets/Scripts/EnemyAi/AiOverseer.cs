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

    public void AppendAiToRoom(Rooms roomIndex, GenericEnemyHandler genericEnemyHandler)
    {
        // Place a check to see if the room is already full

        for(int i=0; i< roomIDs[(int)roomIndex].genericEnemyHandlers.Length; i++) {

            if (roomIDs[(int)roomIndex].genericEnemyHandlers[i] == null) {
                roomIDs[(int)roomIndex].genericEnemyHandlers[i] = genericEnemyHandler;
                genericEnemyHandler.aiNumber = i;
                genericEnemyHandler.roomIndex = roomIndex;
                roomIDs[(int)roomIndex].full = CheckRoomIfFull(roomIndex);
                return;
            }
        }

        //roomIDs[(int)roomIndex].full = CheckRoomIfFull(roomIndex);
    }

    public void ClearAiFromRoom(Rooms roomIndex, int index)
    {
        roomIDs[(int)roomIndex].genericEnemyHandlers[index] = null;
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
        }

    }

    public int MoveAllAiToRoom(Rooms fromRoom, Rooms toRoom) // returns the number of ai moved to the other room
    {
        if (roomIDs[(int)toRoom].full) return -1;
        int numberMoved = 0;
        
        //if(roomIDs[(int)fromRoom].genericEnemyHandlers == null) 

        for (int i = 0; i < roomIDs[(int)fromRoom].genericEnemyHandlers.Length; i++)
        {
            if(roomIDs[(int)fromRoom].genericEnemyHandlers[i] != null)
            {
                if (roomIDs[(int)toRoom].full)
                {
                    roomIDs[(int)fromRoom].full = CheckRoomIfFull(fromRoom);
                    return numberMoved;
                }

                Vector3 position = RequestDistance(toRoom, Vector3.zero, 0f);
                roomIDs[(int)fromRoom].genericEnemyHandlers[i].Move(position);

                AppendAiToRoom(toRoom, roomIDs[(int)fromRoom].genericEnemyHandlers[i]);
                ClearAiFromRoom(fromRoom, i);
                numberMoved++;
            }
        }

        roomIDs[(int)fromRoom].full = CheckRoomIfFull(fromRoom);
        return numberMoved;
    }

    public int FlankRoomThroughRoom(Rooms flankedRoom, Rooms throughRoom, Rooms fromRoom)
    {
        if (roomIDs[(int)throughRoom].full) return -1;
        int numberMoved = 0;

        //if(roomIDs[(int)fromRoom].genericEnemyHandlers == null) 

        for (int i = 0; i < roomIDs[(int)fromRoom].genericEnemyHandlers.Length; i++)
        {
            if (roomIDs[(int)fromRoom].genericEnemyHandlers[i] != null)
            {
                if (roomIDs[(int)throughRoom].full)
                {
                    roomIDs[(int)fromRoom].full = CheckRoomIfFull(fromRoom);
                    return numberMoved;
                }
                int areaMask = ~(1 << (int)roomIDs[(int)flankedRoom].navMeshAreaMask);
                roomIDs[(int)fromRoom].genericEnemyHandlers[i].navMeshAgent.areaMask &= areaMask;

                Vector3 position = RequestDistance(throughRoom, Vector3.zero, 0f);
                roomIDs[(int)fromRoom].genericEnemyHandlers[i].Move(position);

                roomIDs[(int)fromRoom].genericEnemyHandlers[i].queuedRoom = throughRoom;
                roomIDs[(int)fromRoom].genericEnemyHandlers[i].toRoomQueued = flankedRoom;
                roomIDs[(int)fromRoom].genericEnemyHandlers[i].command = AiOverseerQueuedCommands.Distance;
                roomIDs[(int)fromRoom].genericEnemyHandlers[i].overseerQueuedCommand = true;

                AppendAiToRoom(throughRoom, roomIDs[(int)fromRoom].genericEnemyHandlers[i]);
                ClearAiFromRoom(fromRoom, i);
                numberMoved++;
            }
        }

        roomIDs[(int)fromRoom].full = CheckRoomIfFull(fromRoom);
        return numberMoved;
    }
    /* Definitive Functions */

    public bool CheckRoomIfFull(Rooms roomIndex)
    {
        for(int i=0; i<roomIDs[(int)roomIndex].genericEnemyHandlers.Length; i++)
        {
            if (roomIDs[(int)roomIndex].genericEnemyHandlers[i] == null)
                return false;
        }

        return true;
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
