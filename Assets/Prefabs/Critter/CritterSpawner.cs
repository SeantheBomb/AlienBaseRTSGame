using UnityEngine;
using System.Collections;

public class CritterSpawner : MonoBehaviour {

    [System.Serializable]
    public class WaveComponent
    {
        public bool spawnOutsideCamera=true;
        public GameObject prefab;
        public Vector3 spawnPos = Vector3.zero;
        public int dayAvailable = 0;
        public float spawnCooldown;
        [System.NonSerialized]
        public float spawnCDRemaining;
        [System.NonSerialized]
        public int spawnAmount;
        [System.NonSerialized]
        public SpawnPool pool;

        public void initialize()
        {
            pool = new SpawnPool(prefab);
        }

        public bool canSpawn()
        {
            spawnAmount = (DayNightCycle.day + 1) - dayAvailable;
            spawnCDRemaining -= Time.deltaTime;
            if (spawnCDRemaining < 0)
            {
                spawnCDRemaining = spawnCooldown;
                return true;
            }
            return false;
        }
	}

    public WaveComponent[] waveComps;
    public int density;

    new CameraController camera;


    // Use this for initialization
    void Start()
    {
        Transform parent = GameObject.Find("Wildlife").transform;
        camera = FindObjectOfType<CameraController>();
        foreach (WaveComponent wc in waveComps)
        {
            wc.initialize();
            if (DayNightCycle.day >= wc.dayAvailable)
            {
                if (wc.spawnCooldown == 0)
                    wc.spawnCooldown = (float)(density);
                wc.spawnCDRemaining = wc.spawnCooldown;
                wc.pool.parent = parent;
            }
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {


        foreach (WaveComponent wc in waveComps)
        {
            if (wc.canSpawn())
            {
                //Spawn it!
                for (int i = 0; i < wc.spawnAmount; i++)
                {
                    GameObject spawned;
                    if (wc.spawnPos == Vector3.zero)
                        spawned = wc.pool.spawn(getPos(wc.spawnOutsideCamera));
                    else spawned = wc.pool.spawn(wc.spawnPos);
                    //spawned.transform.parent = this.transform;
                }
            }
        }

    }

    public void Kill(Critter obj)
    {
        foreach (WaveComponent wc in waveComps)
        {
            if (wc.pool.despawn(obj.gameObject))
                return;
        }
        UILog.AddText("Failed to kill" + obj.name + obj.entity.UID);
    }

    public void Kill(int uid)
    {
        foreach (WaveComponent wc in waveComps)
        {
            if (wc.pool.despawn(uid))
                return;
        }
        UILog.AddText("Failed to kill" + uid);
    }

    public Vector3 getPos(bool spawnArea)
    {
        float spawnX, spawnZ;
        if (spawnArea)
        {
            bool spawn = Random.value > 0.5f;
            spawnX = spawn ? Random.Range(-camera.bounds - 20, camera.bounds + 20f) :
                (Random.value > 0.5 ? Random.Range(camera.bounds + 10, camera.bounds + 20f) :
                Random.Range(-camera.bounds - 10, -camera.bounds - 20f));

            spawnZ = !spawn ? Random.Range(-camera.bounds - 20, camera.bounds + 20f) :
                (Random.value > 0.5 ? Random.Range(camera.bounds + 10, camera.bounds + 20f) :
                Random.Range(-camera.bounds - 10, -camera.bounds - 20f));
        }
        else
        {
            spawnX = Random.Range(-Director.mapX, Director.mapX);
            spawnZ = Random.Range(-Director.mapZ, Director.mapZ);
        }

        return new Vector3(spawnX, 0, spawnZ);
    }


}

