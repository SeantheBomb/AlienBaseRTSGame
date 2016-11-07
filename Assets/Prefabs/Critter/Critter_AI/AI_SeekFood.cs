using UnityEngine;
using System.Collections;

public class AI_SeekFood : AI_Behaviour {

    public string critterType = "Vegetable";
    public float eatingRange = 1f;
    public float eatHPPerSec = 5f;
    public float eatHPToEnergy = 0.5f;

    Critter myCritter;

	// Use this for initialization
	void Start () {
        myCritter = GetComponent<Critter>();
	}
	
	protected override void DoAIBehaviour()
    {
        if(Critter.crittersByType.ContainsKey(critterType) == false)
        {
            //We have nothing to eat
            return;
        }

        //Find the closest edible critter to us
        Critter closest = null;
        float dist = Mathf.Infinity;
        foreach(Critter c in Critter.crittersByType[critterType])
        {
            if (c.isHiding)
                continue;//Can't see this critter
            float d = Vector3.Distance(this.transform.position, c.transform.position);
            if(closest == null || d < dist)
            {
                closest = c;
                dist = d;
            }
        }
        if(closest == null)
        {
            //No valid targets exist.
            return;
        }

        if (dist < eatingRange)
        {
            float hpEaten = Mathf.Clamp(eatHPPerSec * Time.deltaTime, 0, closest.entity.health);
            closest.entity.takeDamage(hpEaten);
            myCritter.energy += hpEaten * eatHPToEnergy;
        }
        else
        {
            //Now we want to move towards the closest edible critter
            Vector3 dir = closest.transform.position - this.transform.position;
            //float energyWeight = weight / (myCritter.energy/4 * myCritter.energy/4);
            //GetComponent<Critter>().desiredDirections.Add(new WeightedDirection(dir, energyWeight, type));
            GetComponent<Critter>().desiredDirections.Add(new WeightedDirection(dir, weight, type));
        }
    }
}
