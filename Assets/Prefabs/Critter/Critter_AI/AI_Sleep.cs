using UnityEngine;
using System.Collections;

public class AI_Sleep : AI_Behaviour {

    public bool isNocturnal = false;

    public string hidingSpot = "HidingSpot";

    Critter myCritter;

    // Use this for initialization
    void Start()
    {
        myCritter = GetComponent<Critter>();
    }

    protected override void DoAIBehaviour()
    {
        if (DayNightCycle.timeOfDay == TimeOfDay.Night)//If its Night time, and our critter is nocturnal, then don't sleep
        {
            if (isNocturnal)
                return;
        }
        else
        {
            if (isNocturnal==false)//If it is day time and our creature is not nocturnal, then don't sleep
                return;
        }

        myCritter.isSleeping = true;

        //Now we want to move to the closest hedge
        Nature closest = null;
        float dist = Mathf.Infinity;
        foreach (Nature hs in Nature.NatureByType[hidingSpot])
        {
            float d = Vector3.Distance(this.transform.position, hs.transform.position);
            if (closest == null || d < dist)
            {
                closest = hs;
                dist = d;
            }
        }
        if (closest == null)
        {
            //No hedges
            return;
        }

        Vector3 dir = closest.transform.position - this.transform.position;

        GetComponent<Critter>().desiredDirections.Add(new WeightedDirection(dir, weight, type));
    }
}
