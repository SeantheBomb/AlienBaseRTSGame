using UnityEngine;
using System.Collections;



public class StructureController : MonoBehaviour {

    public Entity entity;

    public int buildingMaterialCost;
    public int powerCost;
    public Resource product;
    public bool isDaily;
    public bool isDepleted;

	// Use this for initialization
	void Start () {
        entity = Entity.RequireEntity(this.gameObject);
        isDepleted = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnLevelUp()
    {
        entity.health += 20;
    }

    public void OnDeath()
    {
        GetComponent<AudioSource>().Play();
    }
}
