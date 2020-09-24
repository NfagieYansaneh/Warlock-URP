using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiOverseer : MonoBehaviour
{
    public GameObject[] pooledAiObjects;
    public RoomID[] roomIDs;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //NavMeshBuilder.BuildNavMeshData();
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

    //Random.insideUnitSphere

    public Vector3 RequestDistance(Rooms roomIndex, Vector3 otherPosition, float radius)
    {
        Debug.Log("Requested Distance");

        float rndX = Random.Range(0f, roomIDs[(int)roomIndex].width - radius);
        Debug.Log(rndX);
        if (rndX > Mathf.Abs(otherPosition.x - radius)) rndX += (otherPosition.x + radius - roomIDs[(int)roomIndex].xzColliderCorners[0]) - (otherPosition.x - (otherPosition.x - radius));

        float rndZ = Random.Range(0f, roomIDs[(int)roomIndex].length - radius);
        Debug.Log(rndZ);
        if (rndZ > Mathf.Abs(otherPosition.y - radius)) rndZ += (otherPosition.z + radius - roomIDs[(int)roomIndex].xzColliderCorners[1]) - (otherPosition.z - (otherPosition.z - radius));

        Vector3 dest = new Vector3(
                roomIDs[(int)roomIndex].xzColliderCorners[0] + rndX,
                0f,
                roomIDs[(int)roomIndex].xzColliderCorners[1] + rndZ
            );

        Debug.Log(dest);
        return dest;
    }
}
