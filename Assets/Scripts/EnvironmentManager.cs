using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnvironmentManager : MonoBehaviour
{
	public GameObject[] objects;                // The enemy prefab to be spawned.
	public int maxSpawnCount = 3;
	public GameObject[] spawnPoints;         // An array of the spawn points this enemy can spawn from.
    public NavMeshSurface nav;

	private int spawnCount = 0;
	void Start()
	{
		// Call the Spawn function after a delay of the spawnTime and then continue to call after the same amount of time.
		spawnPoints = GameObject.FindGameObjectsWithTag("objectSpawn");
        //shuffle array
        shuffle<GameObject>(spawnPoints);
		Spawn();
        nav.BuildNavMesh();
	}


	void Spawn()
	{
		// If the player has no health left...
		//if (playerHealth.currentHealth <= 0f)
		//{
		//    // ... exit the function.
		//    return;
		//}
		if (spawnCount == maxSpawnCount) return;
       

        // Find a random index between zero and one less than the number of spawn points.
        //int spawnPointIndex = Random.Range(0, spawnPoints.Length);
        int objectIndex;

        // Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
        for (int i = 0; i < maxSpawnCount; i++)
		{
            objectIndex = Random.Range(0, objects.Length);
            Quaternion randomRotation = new Quaternion(0, Random.Range(0, 360), 0, 0);

            var rock = Instantiate(objects[objectIndex], spawnPoints[i].transform.position, randomRotation);
            rock.gameObject.tag = "rock";
		}
	}


    public static void shuffle<T>(T[] arr)
    {
        for (int i = arr.Length - 1; i > 0; i--)
        {
            int r = Random.Range(0, i);
            T tmp = arr[i];
            arr[i] = arr[r];
            arr[r] = tmp;
        }
    }
        
    public void SpawnTerrain()
    {
        GameObject[] rocks = GameObject.FindGameObjectsWithTag("rock");
        for(int i = 0; i < rocks.Length; i++)
        {
            Destroy(rocks[i]);
        }
        //shuffle array
        shuffle<GameObject>(spawnPoints);
        Spawn();
        
    }
}
