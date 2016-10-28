using UnityEngine;
using System.Collections;

public class AI_HideFromCritter : AI_Behaviour {

    public string critterType = "Carnivore";
    public string hidingSpot = "HidingSpot";
    public float fearDistance = 5f;

    Critter myCritter;

    // Use this for initialization
    void Start()
    {
        myCritter = GetComponent<Critter>();
    }

    protected override void DoAIBehaviour()
    {
        if (Critter.crittersByType.ContainsKey(critterType) == false)
        {
            //We have nothing to eat
            return;
        }

        bool predatorNearby = false;

        //Find the closest edible critter to us
        foreach (Critter c in Critter.crittersByType[critterType])
        {
            float d = Vector3.Distance(this.transform.position, c.transform.position);
            if(d < fearDistance)
            {
                predatorNearby = true;
                break;
            }
        }
        if(predatorNearby == false)
        {
            //Nothing to fear
            return;
        }

        //Now we want to move to the closest hedge
        Nature closest = null;
        float dist = Mathf.Infinity;
        foreach(Nature hs in Nature.NatureByType[hidingSpot])
        {
            float  d = Vector3.Distance(this.transform.position, hs.transform.position);
            if(closest == null || d < dist)
            {
                closest = hs;
                dist = d;
            }
        }
        if(closest == null)
        {
            //No hedges
            return;
        }

        Vector3 dir =  closest.transform.position - this.transform.position ;

        GetComponent<Critter>().desiredDirections.Add(new WeightedDirection(dir, weight, type));
    }
}
