using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour {
    public int team;
    public string showName;
    public int UID;
    public static int UIDtick;
    public int level;
    public float maxHealth;
    public float health
    {
        get { return maxHealth - damageTaken; } set { damageTaken = maxHealth - value ;}
    }

    public ClickHitBox clickHitBox { get; protected set; }

    private float damageTaken;

	// Use this for initialization
    void Start ()
    {
        setUID();
        GameObject clickHitBoxGO = new GameObject();
        clickHitBoxGO.name = "ClickHitBox";
        clickHitBoxGO.transform.SetParent(this.transform, false);
        clickHitBox = clickHitBoxGO.AddComponent<ClickHitBox>();
        clickHitBox.parent = this;
        clickHitBox.setSize(2, 2);
        clickHitBox.updateSize();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    protected void setUID()
    {
        UID = UIDtick;
        UIDtick++;
    }

    public void takeDamage(float dam)
    {
        BroadcastMessage("OnDamageTaken");
        damageTaken += dam;
        if (health <= 0)
            die();
    }

    public void takeDamage(float dam, int pingUID)
    {
        BroadcastMessage("OnDamageTaken");
        damageTaken += dam;
        if (health <= 0)
            die(pingUID);
    }

    public void die()
    {
        BroadcastMessage("OnDeath");
        Destroy(gameObject);
    }

    public void die(int pingUID)
    {
        BroadcastMessage("OnDeath");
        PingEntity(pingUID);
        Destroy(gameObject);
    }

    public int levelUp()
    {
        BroadcastMessage("OnLevelUp");
        level++;
        return level;
    }

    public bool PingEntity(int pingUID)
    {
        return FindUID(pingUID)!=null;
    }

    public static Entity FindUID(int uid)
    {
        Entity[] allEntity = FindObjectsOfType<Entity>();
        foreach(Entity entity in allEntity)
        {
            if (entity.UID == uid)
                return entity;
        }
        return null;
    }

    public static Entity RequireEntity(GameObject obj)
    {
        if (obj.GetComponent<Entity>() == null)
            return obj.AddComponent<Entity>();
        return obj.GetComponent<Entity>();

    }

}
