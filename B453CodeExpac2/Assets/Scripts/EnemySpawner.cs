using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public float spawnRate;
    public GameObject spawnAnimation;
    public List<GameObject> spawnPool;

    private float tick;

    void Start()
    {
    }

    void Update()
    {
        if (tick >= spawnRate)
        {
            int spawnIndex = Random.Range(0, spawnPool.Count - 1);
            GameObject objToSpawn = spawnPool[spawnIndex];
            if (objToSpawn)
            {
                float spawnY = Random.Range(-4.0f, 4.0f);
                float spawnX = Random.Range(-8.0f, 8.0f);
 
                Vector2 spawnPosition = new Vector2(spawnX, spawnY);
                GameObject target = GameObject.Instantiate(spawnAnimation, spawnPosition, Quaternion.identity);
                target.GetComponent<AnimationDelay>().assignSpawnObject(3, objToSpawn);
            }
            tick = 0;
        }
        tick += UnityEngine.Time.deltaTime;
    }
}