using UnityEngine;
using System.Collections;

public class GuardSlot : MonoBehaviour {

    public UnitController unit;
    public bool filled;

	// Use this for initialization
	void Start () {
        filled = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (unit != null)
            unit.transform.position = transform.position;
	}

    public void loadSlot(UnitController unit)
    {
        this.unit = unit;
        this.unit.guard=this;
        this.unit.waypoint.gameObject.SetActive(false);
        filled = true;
    }

    public UnitController unloadSlot()
    {
        UnitController result = unit;
        this.unit.guard = null;
        this.unit = null;
        filled = false;
        return unit;
    }
}
