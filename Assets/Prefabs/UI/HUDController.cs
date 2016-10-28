using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUDController : MonoBehaviour {

    public Director director;
    public SupplyShipController ssd;

    public Text dayText;
    public Text daysUntilSupplyText;
    public Text timeOfDayText;
    public Text foodText;
    public Text resourcesText;
    public Text suppliesText;
    public Text powerText;

	// Use this for initialization
	void Start () {
        director = FindObjectOfType<Director>();
        ssd = FindObjectOfType<SupplyShipController>();
	}
	
	// Update is called once per frame
	void Update () {

        dayText.text = "Day: " + DayNightCycle.day;
        daysUntilSupplyText.text = "Days Until Supply: " + ssd.daysUntilSupply;
        timeOfDayText.text = DayNightCycle.timeOfDay.ToString();
        foodText.text = "Food: " + Resource.Food.amount.ToString();
        resourcesText.text = "Resources: " + Resource.BuildingMaterial.amount.ToString();
        suppliesText.text = "Supplies: " + Resource.Supplies.amount.ToString();
        powerText.text = "Power: " + Resource.Power.amount.ToString() + "/"+Resource.Power.capacity.ToString();

	}
}
