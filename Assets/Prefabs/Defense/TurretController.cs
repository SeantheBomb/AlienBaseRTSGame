using UnityEngine;
using System.Collections;

public class TurretController : DefenseController
{

    public float range;
    public float damage;
    public float accuracy;
    public float firerate;

    public Transform target;
    public Transform head;

    public GunController gun;

	// Use this for initialization
	void Start () {
        gun = GetComponentInChildren<GunController>();
        //head = GetComponentInChildren<Transform>();
        gun.range = range;
        gun.damage = damage;
        gun.accuracy = accuracy;
        gun.firerate = firerate;
	}
	
	// Update is called once per frame
	void Update () {

        if (target == null || target.gameObject.activeSelf==false)
            target = getClosestEnemy();
        else
        {
            Vector3 dir = target.position - head.position;


            if (dir.magnitude <= gun.range)
            {
                Quaternion targetRotation = Quaternion.LookRotation(dir.normalized);
                this.head.transform.rotation = Quaternion.Lerp(this.head.transform.rotation, targetRotation, Time.deltaTime * 5);
                fireGun();
            }

        }
    }

    public void fireGun()
    {
        gun.shoot();
    }

    float getDistanceFrom(Transform obj)
    {
        return Vector3.Distance(transform.position, obj.transform.position);
    }

    public Transform getClosestEnemy()
    {
        EnemyController[] allEnemys = FindObjectsOfType<EnemyController>();
        EnemyController minEnemy = null;
        float minDist = Mathf.Infinity;
        foreach (EnemyController e in allEnemys)
        {
            if (e.gameObject.activeSelf == false)
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

    public new void OnLevelUp()
    {
        range += 2;
        accuracy += 2;
        damage += 2;
    }
}
