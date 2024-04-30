using UnityEngine;
using System.Collections;
using Unity.FPS.Game;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 2f; // Time between spawns
    public Vector3 spawnArea = new Vector3(10, 0, 10); // Spawn area size
    public int initialMaxEnemies = 5; // Initial maximum number of enemies
    public int maxEnemiesIncrement = 3; // Amount to increase maxEnemies by
    public float increaseInterval = 1000f; // Time interval between maxEnemies increases

    private int maxEnemies; // Maximum number of enemies allowed
    private static int currentEnemyCount = 0; // Current number of active enemies
    private float timeSinceLastIncrease = 0f;

    private void Start()
    {
        maxEnemies = initialMaxEnemies;
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            if (currentEnemyCount < maxEnemies)
            {
                SpawnEnemy();
            }
        }
    }

    private void Update()
    {
        timeSinceLastIncrease += Time.deltaTime;
        if (timeSinceLastIncrease >= increaseInterval)
        {
            IncreaseMaxEnemies();
            timeSinceLastIncrease = 0f;
        }
    }

    private void SpawnEnemy()
    {
        Vector3 spawnPosition = new Vector3(
            Random.Range(-spawnArea.x, spawnArea.x),
            Random.Range(-spawnArea.y, spawnArea.y),
            Random.Range(-spawnArea.z, spawnArea.z)
        ) + transform.position;
        
        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        currentEnemyCount++; // Increase count of active enemies
        Health enemyHealth = enemy.GetComponent<Health>(); // Assuming there's an EnemyHealth script attached
        if (enemyHealth != null)
        {
            enemyHealth.OnDie += HandleEnemyDeath; // Subscribe to the death event
        }
    }

    private void HandleEnemyDeath()
    {
        currentEnemyCount--; // Decrease count of active enemies
    }

    private void IncreaseMaxEnemies()
    {
        maxEnemies += maxEnemiesIncrement;
    }
}
