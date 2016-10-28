using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SupplyShipController : MonoBehaviour {

    public int supplyWait = 30;
    public int daysUntilSupply { get; private set; }

    public int maxVal = 50;

    public bool exceedsCapacity { get; private set; }

    public GameObject supplyRequisitionUI;

    public Image ShowCapacity;
    public Color notFullColor = Color.white;
    public Color fullColor = Color.red;

    public Requisition totalVal, foodVal, materialVal, supplyVal, unitZeroVal;

    public GameObject[] unitPrefab;
    public Transform unitParent;

    int currDay = 0;
    bool UIOpen = true;
    bool supplyShipArrived = false;

    PlayerController player;
    Director director;

	// Use this for initialization
	void Start () {
        player = FindObjectOfType<PlayerController>();
        director = FindObjectOfType<Director>();
        totalVal = supplyRequisitionUI.transform.FindChild("TotalValue").GetComponent<Requisition>();
        foodVal = supplyRequisitionUI.transform.FindChild("FoodValue").GetComponent<Requisition>();
        materialVal = supplyRequisitionUI.transform.FindChild("ResourcesValue").GetComponent<Requisition>();
        supplyVal = supplyRequisitionUI.transform.FindChild("SuppliesValue").GetComponent<Requisition>();
        unitZeroVal = supplyRequisitionUI.transform.FindChild("UnitValue").GetComponent<Requisition>();
        SetMaxVal(maxVal);
        daysUntilSupply = supplyWait;
        currDay = DayNightCycle.day;
	}
	
	// Update is called once per frame
	void Update () {
        if (isNewDay())
        {
            if (daysUntilSupply > 1)
                daysUntilSupply--;
            else //Supply Ship has arrived
            {
                daysUntilSupply = 0;
                supplyShipArrived = true;
            }
        }
        if(daysUntilSupply == 0 && supplyShipArrived && DayNightCycle.timeOfDay==TimeOfDay.Noon)
        {
            Resource.addResource(Resource.Type.Food,foodVal.getValue());
            Resource.addResource(Resource.Type.BuildingMaterial, materialVal.getValue());
            Resource.addResource(Resource.Type.Supplies, supplyVal.getValue());
            for (int i = 0; i < unitZeroVal.getValue(); i++)
            {
                spawnUnit(0, new Vector3(Random.Range(3, 10), 0, Random.Range(3, 10))).transform.SetParent(unitParent);
            }
            showUI(true);
            supplyShipArrived = false;
        }
        if (UIOpen)
            updateTotal();   
	
	}

    bool isNewDay()
    {
        if (DayNightCycle.day == currDay)
            return false;
        currDay = DayNightCycle.day;
        return true;
    }


    void updateTotal()
    {
        int total = foodVal.getValue() + materialVal.getValue() + supplyVal.getValue() + unitZeroVal.getValueNoStep();
        //UILog.AddText("Total: " + total);
        exceedsCapacity = total > maxVal;
        ShowCapacity.color = exceedsCapacity ? fullColor : notFullColor;
        setTotal(total);
        
    }

    public void launch()
    {
        if (exceedsCapacity)
        {
            UILog.AddText("Can't launch! Over Capacity!");
            return;
        }
        showUI(false);
        daysUntilSupply = supplyWait;
        UILog.AddText("Supply Shipped launched! Arriving in " + daysUntilSupply + " days.");
    }

    public void showUI(bool show)
    {
        supplyRequisitionUI.SetActive(show);
        UIOpen = show;
    }

    public void setTotal(int val)
    {
        totalVal.setValue(val);
    }

    public int getTotal()
    {
        return totalVal.getValue();
    }

    public void SetMaxVal(int val)
    {
        maxVal = val;

        totalVal.setMaxValue(maxVal);

        foodVal.setMaxValue(maxVal);

        materialVal.setMaxValue(maxVal);

        supplyVal.setMaxValue(maxVal);
    }

    public GameObject spawnUnit(int type, Vector3 position)
    {
        return (GameObject)Instantiate(unitPrefab[type], position, transform.rotation);
    }
}
