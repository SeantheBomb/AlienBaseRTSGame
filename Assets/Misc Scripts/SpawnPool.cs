using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnPool {

    GameObject prefab;
    List<GameObject> pool;
    public Transform parent;

    public SpawnPool()
    {
        setPrefab(null);
    }

    public SpawnPool(GameObject spawnPrefab)
    {
        setPrefab(spawnPrefab);
    }

    public void setPrefab(GameObject spawnPrefab)
    {
        prefab = spawnPrefab;
        if (spawnPrefab != null)
            prefab.SetActive(false);
        else Debug.Log("Warning: Null Prefab");
        pool = new List<GameObject>();
    }

    public GameObject spawn(Transform pos)
    {
        if (pool.Count > 0)
            foreach (GameObject inst in pool)
        {
            if (!inst.activeSelf)
            {
                inst.transform.position = pos.position;
                inst.transform.rotation = pos.rotation;
                inst.SetActive(true);
                return inst;
            }
        }
        GameObject clone = (GameObject)GameObject.Instantiate(prefab, pos.position, pos.rotation);
        clone.SetActive(true);
        if (parent != null)
            clone.transform.SetParent(parent);
        pool.Add(clone);
        return pool[pool.Count - 1];
    }

    public GameObject spawn(Vector3 pos)
    {
        if(pool.Count>0)
        foreach (GameObject inst in pool)
        {
            if (!inst.activeSelf)
            {
                inst.transform.position = pos;
                inst.transform.rotation = Quaternion.identity;
                inst.SetActive(true);
                return inst;
            }
        }
        GameObject clone = (GameObject)GameObject.Instantiate(prefab, pos, Quaternion.identity);
        clone.SetActive(true);
        if (parent != null)
            clone.transform.SetParent(parent);
        pool.Add(clone);
        return pool[pool.Count - 1];
    }

    public GameObject spawn(Vector3 pos, Quaternion rot)
    {
        if (pool.Count > 0)
            foreach (GameObject inst in pool)
        {
            if (!inst.activeSelf)
            {
                inst.transform.position = pos;
                inst.transform.rotation = rot;
                inst.SetActive(true);
                return inst;
            }
        }
        GameObject clone = (GameObject)GameObject.Instantiate(prefab, pos, rot);
        clone.SetActive(true);
        if (parent != null)
            clone.transform.SetParent(parent);
        pool.Add(clone);
        return pool[pool.Count - 1];
    }

    public GameObject spawn(GameObject prefab, Transform pos)
    {
        this.prefab = prefab;
        if (pool.Count > 0)
            foreach (GameObject inst in pool)
            {
                if (!inst.activeSelf)
                {
                    inst.transform.position = pos.position;
                    inst.transform.rotation = pos.rotation;
                    inst.SetActive(true);
                    return inst;
                }
            }
        GameObject clone = (GameObject)GameObject.Instantiate(prefab, pos.position, pos.rotation);
        clone.SetActive(true);
        if (parent != null)
            clone.transform.SetParent(parent);
        pool.Add(clone);
        return pool[pool.Count - 1];
    }

    public GameObject spawn(GameObject prefab, Vector3 pos)
    {
        this.prefab = prefab;
        if (pool.Count > 0)
            foreach (GameObject inst in pool)
            {
                if (!inst.activeSelf)
                {
                    inst.transform.position = pos;
                    inst.transform.rotation = Quaternion.identity;
                    inst.SetActive(true);
                    return inst;
                }
            }
        GameObject clone = (GameObject)GameObject.Instantiate(prefab, pos, Quaternion.identity);
        clone.SetActive(true);
        if (parent != null)
            clone.transform.SetParent(parent);
        pool.Add(clone);
        return pool[pool.Count - 1];
    }

    public GameObject spawn(GameObject prefab, Vector3 pos, Quaternion rot)
    {
        this.prefab = prefab;
        if (pool.Count > 0)
            foreach (GameObject inst in pool)
            {
                if (inst == null)
                    continue;
                if (!inst.activeSelf)
                {
                    inst.transform.position = pos;
                    inst.transform.rotation = rot;
                    inst.SetActive(true);
                    return inst;
                }
            }
        GameObject clone = (GameObject)GameObject.Instantiate(prefab, pos, rot);
        clone.SetActive(true);
        if(parent!=null)
            clone.transform.SetParent(parent);
        pool.Add(clone);
        return pool[pool.Count - 1];
    }

    public bool despawn(GameObject obj)
    {
        GameObject targ = pool.Find(o => o.GetInstanceID() == obj.GetInstanceID());
        if (targ == null)
            return false;

        Debug.Log(targ.name + " despawning " + obj.GetInstanceID());
        GameObject clone;
        resetToPrefab(out clone);
        targ.SetActive(false);
        targ = clone;
        return true;


    }

    

    public bool despawn(int uid)
    {
        Entity obj = this.toEntity().Find(e => e.UID == uid);
        if (obj == null)
            return false;
        return despawn(obj.gameObject);
    }


    public void resetToPrefab(out GameObject instance)
    {
        instance = prefab;
    }

    public void clear()
    {
        pool.Clear();
    }

    public List<Entity> toEntity()
    {
        List<Entity> list = new List<Entity>();

        foreach (GameObject obj in pool)
        {
            Entity ent;
            if (Type<Entity>.isType(obj, out ent))
            {
                list.Add(ent);
            }
        }
        return list;
    }

}
