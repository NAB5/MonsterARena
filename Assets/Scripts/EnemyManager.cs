using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemy;                // The enemy prefab to be spawned.
    public GameObject boss;
    public float spawnTime = 3f;            // How long between each spawn.
    public int maxSpawnCount = 4;
    public GameObject[] spawnPoints;         // An array of the spawn points this enemy can spawn from.

    private int spawnCount = 0;
    void Start()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("enemySpawn");
        // Call the Spawn function after a delay of the spawnTime and then continue to call after the same amount of time
        
    }


    public void Spawn()
    {
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);
        Instantiate(enemy, spawnPoints[spawnPointIndex].transform.position, spawnPoints[spawnPointIndex].transform.rotation);
        
    }

    public void SpawnBoss()
    {
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);
        Instantiate(boss, spawnPoints[spawnPointIndex].transform.position, spawnPoints[spawnPointIndex].transform.rotation);

    }

    //void SpawnHelper()
    //{
    //    // If the player has no health left...
    //    //if (playerHealth.currentHealth <= 0f)
    //    //{
    //    //    // ... exit the function.
    //    //    return;
    //    //}
    //    if (spawnCount == maxSpawnCount) return;
    //    // Find a random index between zero and one less than the number of spawn points.
    //    int spawnPointIndex = Random.Range(0, spawnPoints.Length);

    //    // Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
    //    Instantiate(enemy, spawnPoints[spawnPointIndex].transform.position, spawnPoints[spawnPointIndex].transform.rotation);
    //    }

}
