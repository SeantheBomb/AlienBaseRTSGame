using UnityEngine;
using System.Collections;

public class EnemySpawn : MonoBehaviour {



    [System.Serializable]
    public class WaveComponent{
        public GameObject enemyPrefab;
        public Vector3 spawnPos = Vector3.zero;
        public int dayAvailable = 0;
        public float spawnCooldown;
        [System.NonSerialized]
        public float spawnCDRemaining;
        [System.NonSerialized]
        public int spawnAmount;
        [System.NonSerialized]
        public SpawnPool enemyPool;

       public void initialize()
        {
            enemyPool=new SpawnPool(enemyPrefab);
        }

        public bool canSpawn()
        {
            spawnAmount = (DayNightCycle.day+1) - dayAvailable;
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
    public int difficulty = 5;

    new CameraController camera;


    // Use this for initialization
    void Start()
    {
        Transform parent = GameObject.Find("Enemies").transform;
        camera = FindObjectOfType<CameraController>();
        foreach (WaveComponent wc in waveComps)
        {
            wc.initialize();
            if (DayNightCycle.day >= wc.dayAvailable)
            {
                if (wc.spawnCooldown == 0)
                    wc.spawnCooldown = (float)(difficulty) / 5;
                wc.spawnCDRemaining = wc.spawnCooldown;
                wc.enemyPool.parent = parent;
            }
        }
	}

    // Update is called once per frame
    void LateUpdate()
    {
        if (DayNightCycle.timeOfDay == TimeOfDay.Night)
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
                            spawned = wc.enemyPool.spawn(getPos());
                        else spawned = wc.enemyPool.spawn(wc.spawnPos);
                        //spawned.transform.parent = this.transform;
                    }
                }
            }
        }
    }

    public void Kill(EnemyController obj)
    {
        foreach(WaveComponent wc in waveComps)
        {
            if (wc.enemyPool.despawn(obj.gameObject))
                return;
        }
        UILog.AddText("Failed to kill" + obj.name + obj.entity.UID);
    }

    public void Kill(int uid)
    {
        foreach (WaveComponent wc in waveComps)
        {
            if (wc.enemyPool.despawn(uid))
                return;
        }
        UILog.AddText("Failed to kill" + uid);
    }

    public Vector3 getPos()
    {
        bool spawn = Random.value > 0.5f;
        float spawnX = spawn ? Random.Range(-camera.bounds - 20, camera.bounds + 20f) : 
            (Random.value > 0.5 ? Random.Range(camera.bounds+10, camera.bounds + 20f) : 
            Random.Range(-camera.bounds-10, -camera.bounds - 20f));

        float spawnZ = !spawn ? Random.Range(-camera.bounds - 20, camera.bounds + 20f) :
            (Random.value > 0.5 ? Random.Range(camera.bounds+10, camera.bounds + 20f) :
            Random.Range(-camera.bounds - 10, -camera.bounds - 20f));

        return new Vector3(spawnX, 0, spawnZ);
    }


}
