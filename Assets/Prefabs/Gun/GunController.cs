using UnityEngine;
using System.Collections;

public class GunController : MonoBehaviour {

    public float damage;
    public float accuracy;
    public float range;
    public float firerate;

    public Transform BulletSpawn;

    Light torch;

    private float cooldown;
    private Transform parent;
    private Entity parentEntity;
    private int parentUID;

	// Use this for initialization
	void Start () {
        parent = transform;
        while(parentEntity == null)
        {
            if (parent.parent == null)
                break;
            parent = parent.parent;
            parentEntity = parent.GetComponent<Entity>();
        }
        parent = transform.parent;
        parentUID = transform.GetComponentInParent<Entity>().UID;
        BulletSpawn = transform.GetChild(0);
        cooldown = 0;
        torch = GetComponentInChildren<Light>();
    }
	
	// Update is called once per frame
	void Update () {
        if (cooldown > 0)
            cooldown -= Time.deltaTime;
        if (DayNightCycle.timeOfDay == TimeOfDay.Night)
            torch.intensity = 8;
        else
            torch.intensity = 0;
    }

    public Entity shoot()
    {
        Entity reciever=null;
        if (cooldown <= 0)
        {
            //Vector3 dir = new Vector3 (BulletSpawn.forward.x, BulletSpawn.forward.y, BulletSpawn.forward.z+(Random.Range(-accuracy, accuracy)));
            Vector3 dir = parent.forward;
            //Vector3 pos = BulletSpawn.position + (BulletSpawn.forward/10);
            Vector3 pos = BulletSpawn.position;

            RaycastHit hit;
            Ray ray = new Ray(pos, dir.normalized);
            Debug.DrawRay(pos, dir.normalized, Color.red);
            GetComponent<AudioSource>().Play();
            if(Physics.Raycast(ray, out hit, range)){
                if (Type<Entity>.isType(hit.transform, out reciever))
                {
                    if (parentEntity.team == reciever.team)
                        return null;
                    EnemyController enemy;
                    if (Type<EnemyController>.isType(reciever.transform, out enemy))
                        enemy.takeDamage(damage, parentUID);
                    else
                        reciever.takeDamage(damage);
                }
            }

            cooldown = firerate;
            
        }
        return reciever;
    }
}
