using UnityEngine;
using System.Collections;

public class WoodNode : ResourceNode {

	// Use this for initialization
	void Start () {
        type = Resource.Type.BuildingMaterial;
        resource = (int)(maxHealth);
        base.Start();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
