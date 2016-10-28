using UnityEngine;
using System.Collections;

[System.Serializable]
public class Resource
{
    public enum Type
    {
        Food,
        BuildingMaterial,
        Supplies,
        Power
    }

    public static Resource Food = new Resource(Type.Food);
    public static Resource BuildingMaterial = new Resource(Type.BuildingMaterial);
    public static Resource Supplies = new Resource(Type.Supplies);
    public static Resource Power = new Resource(Type.Power);

    public Type type;
    public int capacity;
    public int amount;

    public Resource(Type type)
    {
        this.type = type;
        this.capacity = 50;
        this.amount = 0;
    }

    public Resource(Type type, int capacity)
    {
        this.type = type;
        this.capacity = capacity;
        this.amount = 0;
    }

    public static void addResource(Type type, int amount)
    {
        switch (type)
        {
            case Type.Food:
                {
                    Food.amount += amount;
                    return;
                }
            case Type.BuildingMaterial:
                {
                    BuildingMaterial.amount += amount;
                    return;
                }
            case Type.Supplies:
                {
                    Supplies.amount += amount;
                    return;
                }
            case Type.Power:
                {
                    Power.amount += amount;
                    return;
                }

        }
    }

    public static void setCapacity(Type type, int capacity)
    {
        switch (type)
        {
            case Type.Food:
                {
                    Food.capacity = capacity;
                    return;
                }
            case Type.BuildingMaterial:
                {
                    BuildingMaterial.capacity = capacity;
                    return;
                }
            case Type.Supplies:
                {
                    Supplies.capacity = capacity;
                    return;
                }
            case Type.Power:
                {
                    Power.capacity = capacity;
                    return;
                }

        }
    }

    public static void addCapacity(Type type, int capacity)
    {
        switch (type)
        {
            case Type.Food:
                {
                    Food.capacity += capacity;
                    return;
                }
            case Type.BuildingMaterial:
                {
                    BuildingMaterial.capacity += capacity;
                    return;
                }
            case Type.Supplies:
                {
                    Supplies.capacity += capacity;
                    return;
                }
            case Type.Power:
                {
                    Power.capacity += capacity;
                    return;
                }
        }
    }
}

public class Director : MonoBehaviour {
    public float mapWidth;
    public float mapHeight;
    public static float mapX;
    public static float mapZ;
    public Resource food, buildingMaterial, supplies, power;


    public UnitController[] allUnits;
    public StructureController[] allStrucs;
    public DefenseController[] allDefs;
    public EnemyController[] allEnes;

    private int currDay = 0;

	// Use this for initialization
	void Start () {
        Resource.Food = food;
        Resource.BuildingMaterial = buildingMaterial;
        Resource.Supplies = supplies;
        Resource.Power = power;
        mapX = mapWidth / 2;
        mapZ = mapHeight / 2;
        allUnits = FindObjectsOfType<UnitController>();
        allStrucs = FindObjectsOfType<StructureController>();
        allDefs = FindObjectsOfType<DefenseController>();
        allEnes = FindObjectsOfType<EnemyController>();
    }
	
	// Update is called once per frame
	void Update () {
        if (isNewDay())
        {
            updateList();
            updateResources();
        }
	}

    public void updateList()
    {
        allUnits = FindObjectsOfType<UnitController>();
        allStrucs = FindObjectsOfType<StructureController>();
        allDefs = FindObjectsOfType<DefenseController>();
        allEnes = FindObjectsOfType<EnemyController>();
    }

    public void updateResources()
    {
        foreach (StructureController struc in allStrucs)
        {
            if (struc.product.type == Resource.Type.Power)
            {
                Resource.addCapacity(struc.product.type, struc.product.capacity);
            }
            else
            {
                Resource.addResource(struc.product.type, struc.product.amount);
            }
            UILog.AddText("Supplying " + struc.name);
        }
    }

    bool isNewDay()
    {
        if (DayNightCycle.day == currDay)
            return false;
        currDay = DayNightCycle.day;
        Resource.addResource(Resource.Type.Food, -allUnits.Length);
        return true;
    }

    public static bool FlipCoin()
    {
        return Random.value > 0.5;
    }

}
