using UnityEngine;
using System.Collections;

public class MainBaseController : StructureController {

	// Use this for initialization
	void Start () {
        setUID();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision other)
    {
        Debug.Log("Hit from build!");
    }
}
