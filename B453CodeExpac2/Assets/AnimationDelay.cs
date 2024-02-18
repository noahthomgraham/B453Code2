using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationDelay : MonoBehaviour
{
    public float spawnDelay;

    public GameObject itemToBeSpawn;
    
    IEnumerator spawn()
    {
        yield return new WaitForSeconds(spawnDelay);
        GameObject.Instantiate(itemToBeSpawn, transform.position, Quaternion.identity);
        
        // decompose spawn animation effect
        Destroy(gameObject);
    }

    public void assignSpawnObject(float spawnDelay, GameObject itemToBeSpawn)
    {
        this.spawnDelay = spawnDelay;
        this.itemToBeSpawn = itemToBeSpawn;
        StartCoroutine(spawn());
    }
}
