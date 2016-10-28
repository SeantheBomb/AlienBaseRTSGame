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
	protected void Start () {
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

    public virtual void takeDamage(float dam)
    {
        damageTaken += dam;
        if (health <= 0)
            die();
    }

    public virtual void takeDamage(float dam, int pingUID)
    {
        damageTaken += dam;
        if (health <= 0)
            die(pingUID);
    }

    public virtual void die()
    {
        Destroy(gameObject);
    }

    public virtual void die(int pingUID)
    {
        PingEntity(pingUID);
        Destroy(gameObject);
    }

    public virtual int levelUp()
    {
        level++;
        return level;
    }

    public virtual bool PingEntity(int pingUID)
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

}
