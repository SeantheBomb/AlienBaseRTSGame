using UnityEngine;
using System.Collections;

public class AI_Wander : AI_Behaviour {

    Critter myCritter;
    Vector3 dir;
    public float RedirectWaitMin, RedirectWaitMax;
    float redirWait;
    float redirWaitRemaining;

    // Use this for initialization
    void Start()
    {
        myCritter = GetComponent<Critter>();
        dir = Random.onUnitSphere;
        dir.y = 0;
        redirWait = Random.Range(RedirectWaitMin, RedirectWaitMax);
        redirWaitRemaining = redirWait;
    }

    void Update()
    {
        redirWaitRemaining -= Time.deltaTime;
        if(redirWaitRemaining <= 0)
        {
            dir = Random.onUnitSphere;
            dir.y = 0;
            redirWait = Random.Range(RedirectWaitMin, RedirectWaitMax);
            redirWaitRemaining = redirWait;
        }
    }

    protected override void DoAIBehaviour()
    {
        Vector3 direction = dir;

        WeightedDirection wd = new WeightedDirection(dir, weight, type);
        myCritter.desiredDirections.Add(wd);
    }

}
