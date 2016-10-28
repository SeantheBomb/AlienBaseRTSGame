using UnityEngine;
using System.Collections;

public class AI_AvoidObstacle : AI_Behaviour {

    public float distBuffer;

    static NavMeshObstacle[] obstacles;

    Vector3 dir = Vector3.zero;

    Critter myCritter;

    // Use this for initialization
    void Start()
    {
        dir = Director.FlipCoin() ? transform.right : -transform.right;
        obstacles = FindObjectsOfType<NavMeshObstacle>();
        myCritter = GetComponent<Critter>();
    }

    protected override void DoAIBehaviour()
    {

        foreach(NavMeshObstacle o in obstacles)
        {
            if (o == null)
                return;
            float dist = Vector3.Distance(o.transform.position, this.transform.position);
            if (dist <= distBuffer)
            {
                obstacles = moveToTop(o, obstacles);
                float distWeight = weight / (dist * dist);
                GetComponent<Critter>().desiredDirections.Add(new WeightedDirection(dir, distWeight, type));
                return;
            }
            else dir = Director.FlipCoin() ? transform.right : -transform.right;
        }
	
	}

    public NavMeshObstacle[] moveToTop(NavMeshObstacle t, NavMeshObstacle[] tList)
    {
        NavMeshObstacle[] clone = tList;
        int indexFound=-1;
        for(int i=0; i < clone.Length; i++)//Find object
        {
            if (clone[i].gameObject.GetInstanceID() == t.gameObject.GetInstanceID())
            {
                indexFound = i;
                break;
            }
        }
        clone[indexFound] = null;
        for(int i = 1; i < indexFound; i++)
        {
            clone[i] = clone[i - 1];
        }
        clone[0] = t;
        //Debug.Log(tList.ToString());
        //Debug.Log(clone.ToString());
        return clone;
    }
}
