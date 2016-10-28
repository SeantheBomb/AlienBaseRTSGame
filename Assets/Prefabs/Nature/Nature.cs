using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Nature : Entity {

    public string NatureType = "Hedge";

    public Resource resourceType;
    public int resource;

    static public Dictionary<string, List<Nature>> NatureByType;

    private Transform model;

    // Use this for initialization
    void Start () {
        base.Start();
        //Make sure we're in the NatureByType list
        if (NatureByType == null)
        {
            NatureByType = new Dictionary<string, List<Nature>>();
        }
        if (NatureByType.ContainsKey(NatureType) == false)
        {
            NatureByType[NatureType] = new List<Nature>();
        }
        NatureByType[NatureType].Add(this);
        model = transform.GetChild(0);
    }

    public override void die()
    {
        NatureByType[NatureType].Remove(this);
        base.die();
    }

    public override void die(int pingUID)
    {
        //Remove us from the crittersByType list
        NatureByType[NatureType].Remove(this);
        base.die(pingUID);
    }

    // Update is called once per frame
    void Update () {
	
	}
}
