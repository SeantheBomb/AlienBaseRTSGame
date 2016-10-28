using UnityEngine;
using System.Collections;

public class GuardPostController : DefenseController {

    public GuardSlot[] guardSlots;


	// Use this for initialization
	void Start () {
        setUID();
        guardSlots = gameObject.GetComponentsInChildren<GuardSlot>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void loadUnits(UnitController[] units)
    {
        int i = 0;
        foreach(UnitController u in units)
        {
            if (u.atGuard())
                continue;
            if (i >= guardSlots.Length)
                return;
            while (guardSlots[i].filled)
            {
                i++;
                if (i >= guardSlots.Length)
                    return;
            }
                
            guardSlots[i].loadSlot(u);
            i++;
        }
    }



    public UnitController[] unloadUnits()
    {
        UnitController[] units = new UnitController[guardSlots.Length];
        int i = 0;
        foreach(GuardSlot g in guardSlots)
        {
            units[i] = g.unloadSlot();
            i++;
        }
        return units;
    }

    public override void die()
    {
        Debug.Log("Guard Post destroyed!");
        foreach (GuardSlot g in guardSlots)
        {
            if(g.unit != null)
                g.unit.die();
        }
        Destroy(gameObject);
    }
}
