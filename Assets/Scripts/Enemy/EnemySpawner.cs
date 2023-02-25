using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int numEnemiesToSpawn = 5;

    private int numEnemiesAlive = 0; // The number of enemies currently alive in the scene

    public Transform[] spawnPoints; // The locations where enemies can spawn

    public static EnemySpawner instance; // Static reference to the single instance of the EnemySpawner script

    private int numEnemiesKilled = 0;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        SpawnEnemies();
    }

    public void SpawnEnemies()
    {
        for (int i = 0; i < numEnemiesToSpawn; i++)
        {
            // Choose a random spawn point
            int spawnIndex = Random.Range(0, spawnPoints.Length);
            Transform spawnPoint = spawnPoints[spawnIndex];
            Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
            numEnemiesAlive++;
        }
    }

    // Called when an enemy dies
    public void OnEnemyDeath()
    {
        numEnemiesAlive--;
        numEnemiesKilled++;
        if (numEnemiesAlive <= 0)
        {
            numEnemiesToSpawn = numEnemiesToSpawn + numEnemiesToSpawn / 2;
            SpawnEnemies();
        }
    }
}
