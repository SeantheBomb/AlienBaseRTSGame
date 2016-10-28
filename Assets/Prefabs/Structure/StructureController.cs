using UnityEngine;
using System.Collections;



public class StructureController : Entity {

    public int buildingMaterialCost;
    public int powerCost;
    public Resource product;
    public bool isDaily;
    public bool isDepleted;

	// Use this for initialization
	void Start () {
        base.Start();
        isDepleted = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override int levelUp()
    {
        health += 20;
        return base.levelUp();
    }

    public override void die()
    {
        GetComponent<AudioSource>().Play();
        base.die();
    }
}
