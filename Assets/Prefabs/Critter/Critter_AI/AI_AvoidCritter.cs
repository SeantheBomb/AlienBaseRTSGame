using UnityEngine;
using System.Collections;

public class AI_AvoidCritter : AI_Behaviour {

    public string critterType = "Carnivore";

    Critter myCritter;

    // Use this for initialization
    void Start()
    {
        myCritter = GetComponent<Critter>();
    }

    protected override void DoAIBehaviour()
    {
        if (myCritter.isHiding == true)//No need to run if we're in hiding
            return;
        if (Critter.crittersByType.ContainsKey(critterType) == false)
        {
            //We have nothing to eat
            return;
        }

        //Find the closest edible critter to us
        Critter closest = null;
        float dist = Mathf.Infinity;
        foreach (Critter c in Critter.crittersByType[critterType])
        {
            float d = Vector3.Distance(this.transform.position, c.transform.position);
            if (closest == null || d < dist)
            {
                closest = c;
                dist = d;
            }
        }
        if (closest == null)
        {
            //No valid targets exist.
            return;
        }

        //Now we want to move towards the closest edible critter
        Vector3 dir = this.transform.position - closest.transform.position;

        float distWeight = weight / (dist * dist);

        GetComponent<Critter>().desiredDirections.Add(new WeightedDirection(dir, distWeight, type));
    }
}


