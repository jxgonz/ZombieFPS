using UnityEngine;
using System.Collections;
using Unity.FPS.Game;

public class TurretSpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Prefab to spawn
    public float spawnDelay = 300f; // Delay before first spawn (5 minutes)
    public float respawnDelay = 60f; // Delay before respawning after death (1 minute)
    public bool enemyIsActive = false; // Flag to check if the enemy is currently active

    // private float timeSinceStart = 0f; // Time since the game started
    private float timeSinceLastDeath = 0f; // Time since the last enemy died
    private Health enemyHealth;

    private void Start()
    {
        StartCoroutine(InitialSpawnDelay());
    }

    private IEnumerator InitialSpawnDelay()
    {
        yield return new WaitForSeconds(spawnDelay);
        SpawnEnemy();
    }

    private void Update()
    {
        if (!enemyIsActive && timeSinceLastDeath >= respawnDelay)
        {
            SpawnEnemy();
            timeSinceLastDeath = 0f; // Reset the timer after spawning
        }
        else if (!enemyIsActive)
        {
            timeSinceLastDeath += Time.deltaTime; // Increment the death timer only if no enemy is active
        }
    }

    private void SpawnEnemy()
    {
        Vector3 spawnPosition = transform.position; // Assuming you want to spawn at a specific position
        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        enemyHealth = enemy.GetComponent<Health>();
        enemyIsActive = true;
         // Assuming there's a Health script attached
        if (enemyHealth != null)
        {
            enemyHealth.OnDie += HandleEnemyDeath; // Subscribe to the death event
        }
    }

    private void HandleEnemyDeath()
    {
        enemyIsActive = false; // Set flag to false to allow new spawn
        if (enemyHealth != null)
        {
            enemyHealth.OnDie -= HandleEnemyDeath; // Unsubscribe from the event to prevent memory leaks
        }
    }
}