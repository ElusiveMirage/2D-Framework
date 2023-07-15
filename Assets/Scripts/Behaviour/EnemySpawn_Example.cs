using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn_Example : MonoBehaviour
{
    public static int enemiesSpawned;
    public static bool startSpawn = false;
    //==========================================//
    [SerializeField] private float spawnInterval;
    [SerializeField] private float lastSpawnTime;
    //==========================================//
    [SerializeField] private List<GameObject> enemyPrefabs;

    // Start is called before the first frame update
    void Start()
    {
        lastSpawnTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (!startSpawn)
            return;

        if (startSpawn)
        {
            if (Time.time > lastSpawnTime + spawnInterval)
            {
                int r = Random.Range(0, enemyPrefabs.Count);

                Instantiate(enemyPrefabs[r], transform.position, Quaternion.identity);

                lastSpawnTime = Time.time;
                enemiesSpawned++;
            }
        }       
    }
}
