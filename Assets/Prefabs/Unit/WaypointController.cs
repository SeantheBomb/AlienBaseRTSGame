using UnityEngine;
using System.Collections;

public class WaypointController : MonoBehaviour {

    public int UID;
    Direction dir;
    public bool ready;
    bool firstIt=true;

    // Use this for initialization
    void Start () {
        
        ready = true;
    }
	
	// Update is called once per frame
	void LateUpdate () {
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.GetComponent<StructureController>() || other.transform.GetComponent<WaypointController>())
        {
            if (firstIt)
            {
                dir = (Direction)Mathf.CeilToInt(Random.Range(1, 4));
                firstIt = false;
            }
            ready = false;
            
            Debug.Log("Waypoint spawned inside structure. Relocating " + dir.ToString());
        }

    }

    void OnTriggerStay(Collider other)
    {
        if (other.transform.GetComponent<Entity>() || other.transform.GetComponent<WaypointController>())
        {
            Vector3 fixPosition = transform.position;
            float dist=Random.value;
            if (dir == Direction.Up)
            {
                fixPosition.z += dist;
            }
            if (dir == Direction.Down)
            {
                fixPosition.z -= dist;
            }
            if (dir == Direction.Left)
            {
                fixPosition.x -= dist;
            }
            if (dir == Direction.Right)
            {
                fixPosition.x += dist;
            }

            transform.position = fixPosition;
        }
    }

    void OnTriggerExit(Collider other)
    {
        ready = true;
        firstIt = true;
    }
}
