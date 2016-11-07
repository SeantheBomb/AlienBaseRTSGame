using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Critter : MonoBehaviour{

    public Entity entity;

    public float energy = 0;
    public float maxEnergy = 100f;
    public float energyPerSecond = 1f;
    public int xpVal=10;
    public int foodVal=5;
    public float speed;

    public bool isHiding = false;
    public bool isSleeping = false;

    public string critterType = "Vegetable";

    static public Dictionary<string, List<Critter>> crittersByType;

    public List<WeightedDirection> desiredDirections;

    public int lastHitUID = -1;

    Vector3 velocity;

    public Transform model { get; private set; }

    List<Vector3> dirAvgList = new List<Vector3>(5);

	// Use this for initialization
	void Start () {
        entity = Entity.RequireEntity(this.gameObject);
	    //Make sure we're in the crittersByType list
        if(crittersByType == null)
        {
            crittersByType = new Dictionary<string, List<Critter>>();
        }
        if(crittersByType.ContainsKey(critterType) == false)
        {
            crittersByType[critterType] = new List<Critter>();
        }
        crittersByType[critterType].Add(this);
        energy = maxEnergy;
        model = transform.GetChild(0);
	}

    public void OnDeath()
    {
        //Remove us from the crittersByType list
        crittersByType[critterType].Remove(this);
        //Resource.addResource(Resource.Type.Food, foodVal);
        FindObjectOfType<CritterSpawner>().Kill(entity.UID);
    }


    public bool PingEntity(int pingUID)
    {
        try
        {
            UnitController e;
            if (Type<UnitController>.isType(Entity.FindUID(pingUID).gameObject, out e))
            {
                e.target = null;
                e.abilities.killedEnemy(xpVal);
                Debug.Log("Ping found: " + e.entity.showName + " " + e.entity.UID);
                return true;
            }
            return false;
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex);
            return false;
        }

    }

    // Update is called once per frame
    void FixedUpdate () {
        if (speed <= 0 || gameObject.activeSelf==false)
            return;
        if(!isSleeping)//If the critter is sleeping, they're not losing energy
            energy = Mathf.Clamp(energy - Time.deltaTime * energyPerSecond, 0, maxEnergy);

        if(energy <= 0)
        {
            entity.health = Mathf.Clamp(entity.health - Time.deltaTime * energyPerSecond, 0, entity.maxHealth);
        }

        if(entity.health <= 0)
        {
            if (lastHitUID < 0)
                entity.die();
            else
                entity.die(lastHitUID);
            return;
        }

        //Ask all of our AI scripts to tell us in which direction we should move
        desiredDirections = new List<WeightedDirection>();
        BroadcastMessage("DoAIBehaviour", SendMessageOptions.DontRequireReceiver);

        //Add up all the desired directions by weight
        Vector3 dir = Vector3.zero;
        bool doFallback = true;
        foreach(WeightedDirection wd in desiredDirections)
        {
            if (wd.type == WeightedDirection.Type.Blend) {
                dir += wd.direction * wd.weight;
                doFallback = false;
            }
            else if (wd.type == WeightedDirection.Type.Exclusive && wd.direction!=Vector3.zero)
            {
                doFallback = false;
                dir = wd.direction;
                break;
            }
            else if (wd.type == WeightedDirection.Type.Fallback && doFallback)
            {
                dir += wd.direction * wd.weight;
            }
        }

        velocity = Vector3.Lerp(velocity, dir.normalized * speed, Time.deltaTime * 5f);

        transform.Translate(velocity * Time.deltaTime);
        //Rotate towards direction facing
        if (dirAvgList.Count >= dirAvgList.Capacity)
            dirAvgList.Remove(dirAvgList[0]);
        dirAvgList.Add(dir);
        Vector3 dirAvg = Vector3.zero;
        for (int i = 0; i < dirAvgList.Count; i++)
        {
            dirAvg += dirAvgList[i];
        }
        dirAvg /= dirAvgList.Count;
        Quaternion targetRotation = Quaternion.LookRotation(dirAvg.normalized);
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, targetRotation, Time.deltaTime * 5);

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Nature>() != null)
        {
            isHiding = true;
        }
        Debug.Log("Triggered: " + other.name);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Nature>() != null)
        {
            isHiding = false;
        }
    }
}
