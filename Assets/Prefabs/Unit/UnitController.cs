using UnityEngine;
using System.Collections;

public class UnitController : Entity {

    public bool isSelected = false;
    public GuardSlot guard = null;

    public UnitAbilities abilities;

    //public float speed;
    public float range;

    public Transform target = null;

    public GunController gun;

    public Transform waypoint;

    private float savedRange;

    private NavMeshAgent nav;

    Animator anim;

	// Use this for initialization
	void Start () {
        base.Start();
        anim = GetComponentInChildren<Animator>();
        abilities = new UnitAbilities(UID);
        gun = GetComponentInChildren<GunController>();
        waypoint = Instantiate(waypoint);
        waypoint.GetComponent<WaypointController>().UID = UID;
        waypoint.gameObject.SetActive(false);
        savedRange = range;
        nav = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        if (waypoint.gameObject.activeSelf == true && waypoint.GetComponent<WaypointController>().ready)
        {
            moveToWaypoint();
        }
        else
        {
            attackClosestEnemy();
        }
	}


    public bool atGuard()
    {
        return guard != null;
    }

    public void fireGun()
    {
        gun.shoot();
    }

    public void setWaypoint(Vector3 position)
    {
        waypoint.position = position;
        waypoint.gameObject.SetActive(true);
    }

    void setNavPosition(Vector3 pos)
    {
        if (nav.pathEndPosition !=pos)
        {
            anim.SetBool("isMoving", true);
            nav.enabled = true;
            nav.SetDestination(pos);
            nav.Resume();
        }
    }

    public void cancelNav()
    {
        anim.SetBool("isMoving", false);
        waypoint.gameObject.SetActive(false);
        //endNav();
        nav.Stop();
    }


    public Transform getClosestEnemy()
    {
        EnemyController[] allEnemys = FindObjectsOfType<EnemyController>();
        EnemyController minEnemy=null;
        float minDist=Mathf.Infinity;
        foreach(EnemyController e in allEnemys)
        {
            if (e.gameObject.activeSelf==false)
                continue;
            float dist = getDistanceFrom(e.transform);
            if (dist < minDist && dist < range)
            {
                minEnemy = e;
                minDist = dist;
            }
        }
        if (minEnemy == null)
            return null;
        return minEnemy.transform;
    }

    float getDistanceFrom(Transform obj)
    {
        return Vector3.Distance(transform.position, obj.transform.position);
    }

    void OnCollisonEnter(Collision other)
    {
        Debug.Log("Hit!");
        if (Type<StructureController>.isType(other.transform))
        { 
            waypoint.gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        WaypointController w;
        if (Type<WaypointController>.isType(other.transform, out w))
        {
            if (w.UID == UID)
                cancelNav();
        }
    }

    public override int levelUp()
    {
        SendMessage("levelUpAbilities", SendMessageOptions.DontRequireReceiver);
        return base.levelUp();
    }

    public override void takeDamage(float dam)
    {
        GetComponent<AudioSource>().Play();
        base.takeDamage(dam);
    }

    public override void die()
    {
        UILog.AddText("Unit: " + UID.ToString() + " has died!");
        Destroy(waypoint.gameObject);
        Destroy(gameObject);
    }

    private void attackClosestEnemy()
    {
        if (atGuard())
            range = gun.range;
        else
            range = savedRange;
        if (target == null || target.gameObject.activeSelf == false)
            target = getClosestEnemy();
        else
        {
            Vector3 dir = target.position - transform.position;

            float distThisFrame = nav.speed * Time.deltaTime;

            if (dir.magnitude <= distThisFrame + gun.range)
            {
                cancelNav();
                Quaternion targetRotation = Quaternion.LookRotation(dir);
                this.transform.rotation = Quaternion.Lerp(this.transform.rotation, targetRotation, Time.deltaTime * 5);
                fireGun();
            }
            else
            {
                //Move towards target
                setNavPosition(target.position);
            }
        }
    }

    private void moveToWaypoint()
    {
        Vector3 dir = waypoint.position - transform.position;

        float distThisFrame = nav.speed * Time.deltaTime;

        if (dir.magnitude <= distThisFrame)
        {
            //Reached target
            Debug.Log("Reached Waypoint");
            cancelNav();
        }
        else
        {
            //Move towards target
            setNavPosition(waypoint.position);
        }
    }

}
