using UnityEngine;
using System.Collections;

public class EnemyController : Entity {

    public float damage;
    public int xpVal;
    public float distGap = 0.5f;

    public bool isDead = false;

    public Vector3 spawnPoint;
    public Transform targ = null;

    NavMeshAgent nav;
    Animator anim;

	// Use this for initialization
	void Start () {
        base.Start();
        spawnPoint = transform.position;
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        if (isDead)
            return;

        if (DayNightCycle.timeOfDay != TimeOfDay.Night)
        {
            hideFromSun();
            return;
        }

        attackEnemy();
	}

    public override void die(int pingUID)
    {
        isDead = true;
        targ = null;
        nav.Stop();
        nav.enabled = false;
        anim.SetTrigger("die");
        PingEntity(pingUID);
        Invoke("killThisMoFo", 5f);
        this.enabled = false;
    }

    public override void die()
    {
        isDead = true;
        targ = null;
        nav.Stop();
        nav.enabled = false;
        anim.SetTrigger("die");
        Invoke("killThisMoFo", 5f);
        this.enabled = false;
    }

    public void killThisMoFo()
    {

        FindObjectOfType<EnemySpawn>().Kill(this.UID);
    }

    public override void takeDamage(float dam, int pingUID)
    {
        GetComponent<AudioSource>().Play();
        base.takeDamage(dam, pingUID);
    }

    public override bool PingEntity(int pingUID)
    {
        try
        {
            UnitController e;
            if (Type<UnitController>.isType(FindUID(pingUID).gameObject, out e))
            {
                e.target = null;
                e.abilities.killedEnemy(xpVal);
                Debug.Log("Ping found: " + e.showName + " " + e.UID);
                return true;
            }
            return false;
        }
        catch(System.Exception ex)
        {
            Debug.Log(ex);
            return false;
        }

    }

    void updateTarg()
    {

        UnitController targUnit = null;
        float targUnitDis = Mathf.Infinity;
        StructureController targStruc = null;
        float targStrucDis = Mathf.Infinity;
        DefenseController targBase = null;
        float targBaseDis = Mathf.Infinity;

        float targDis = Mathf.Infinity;

        targ = null;

        //Set targUnit
        UnitController[] allUnits = FindObjectsOfType<UnitController>();
        foreach (UnitController u in allUnits)
        {
            if (u.atGuard())
                continue;
            float dis = getDistanceFrom(u.transform);
            if ((dis<targUnitDis))
            {
                targUnit = u;
                targUnitDis = dis;
            }
        }

        //Set targStruc
        StructureController[] allStrucs = FindObjectsOfType<StructureController>();
        foreach (StructureController s in allStrucs)
        {
            float dis = getDistanceFrom(s.transform);
            if ((dis < targStrucDis))
            {
                targStruc = s;
                targStrucDis = dis;
            }
        }

        //Set targBase
        DefenseController[] allBases = FindObjectsOfType<DefenseController>();
        foreach (DefenseController s in allBases)
        {
            float dis = getDistanceFrom(s.transform);
            if ((dis < targBaseDis))
            {
                targBase = s;
                targBaseDis = dis;
            }
        }
        //Debug.Log("Units Found:"+(targUnit!=null ? targUnit.ToString():"No Units") + " " + (targStruc !=null ? targStruc.ToString():"No Strucs" )+ " " + (targBase != null ? targBase.ToString(): "No bases"));

        //Set targ
        if (targUnitDis < targDis && targUnit != null)
        {
            targ = targUnit.transform;
            targDis = targUnitDis;
        }
        if (targStrucDis < targDis && targStruc != null)
        {
            targ = targStruc.transform;
            targDis = targStrucDis;
        }
        if (targBaseDis < targDis && targBase != null)
        {
            targ = targBase.transform;
            targDis = targBaseDis;
        }
    }

    void setNavPosition(Vector3 pos)
    {
        if (nav.pathEndPosition != pos && nav.isOnNavMesh)
        {
            anim.SetBool("isMoving", true);
            nav.enabled = true;
            nav.SetDestination(pos);
            nav.Resume();
        }
    }

    float getDistanceFrom(Transform obj)
    {
        return Vector3.Distance(transform.position, obj.transform.position);
    }

    private void hideFromSun()
    {
        setNavPosition(spawnPoint);
        Vector3 dir = spawnPoint - transform.position;

        float distThisFrame = nav.speed * Time.deltaTime;

        if (dir.magnitude <= distThisFrame + targ.lossyScale.x + distGap || dir.magnitude <= distThisFrame + targ.lossyScale.z + distGap)
        {
            //Reached target
            nav.Stop();
            anim.SetBool("isMoving", false);
        }
    }

    private void attackEnemy()
    {
        if (targ == null)
        {
            updateTarg();
        }
        else
        {
            Vector3 dir = targ.position - transform.position;

            float distThisFrame = nav.speed * Time.deltaTime;

            if (dir.magnitude <= distThisFrame + targ.lossyScale.x + distGap || dir.magnitude <= distThisFrame + targ.lossyScale.z + distGap)
            {
                //Reached target
                nav.Stop();
                anim.SetBool("isMoving", false);
                anim.SetBool("attack", true);
                if (targ.GetComponent<Entity>().health <= 0)
                    targ = null;
                else
                    targ.GetComponent<Entity>().takeDamage(damage * Time.deltaTime);
            }
            else
            {
                //Move towards target
                anim.SetBool("attack", false);
                setNavPosition(targ.position);
            }
        }
    }
}
