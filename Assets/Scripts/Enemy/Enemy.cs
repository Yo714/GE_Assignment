using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int startingHealth = 10;
    private int currentHealth;

    public GameObject healingPrefab;
    private EnemySpawner enemySpawner;

    public float dropRate = 0.1f; // 10% chance of dropping a healing item

    void Start()
    {
        currentHealth = startingHealth;
        enemySpawner = FindObjectOfType<EnemySpawner>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log(currentHealth);
        if (currentHealth <= 0)
        {
            // Enemy is dead, let the spawner know
            enemySpawner.OnEnemyDeath();

            // Check if we should spawn a healing item
            if (Random.value < dropRate)
            {
                // Spawn healing item
                Instantiate(healingPrefab, transform.position, transform.rotation);
            }

            Destroy(gameObject);
        }
    }
}