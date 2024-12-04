using System.Collections.Generic;
using UnityEngine;

public class Spawner_center : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public GameObject rewardPrefab;

    public Vector3 spawnAreaSize = new Vector3(5, 1, 5); // Width, height, depth of spawn area
    public int maxObjectsPerSpawn = 3; // Maximum number of objects to spawn at once

    public float obstacleScale = 4.0f;
    public float rewardScale = 4.0f;

    private GroundSpawner groundSpawner; // Reference to the ground spawner
    private GameTimeManager gameTimeManager; // Reference to the GameTimeManager
    private int ticksSinceLastSpawn = 0; // Tracks ticks since the last spawn
    private int requiredTicksForSpawn; // Ticks required to spawn

    void Start()
    {
        // Find the ground spawner in the scene or assign it via the Inspector
        groundSpawner = FindObjectOfType<GroundSpawner>();
    }

    public void SpawnObject(GameObject tile)
    {
        // Calculate the spawning area around the tile
        Vector3 tilePosition = tile.transform.position;

        for (int i = 0; i < maxObjectsPerSpawn; i++)
        {
            bool isObstacle = Random.value > 0.5f;

            // Generate a random position within the spawn area
            Vector3 randomOffset = new Vector3(
                Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
                Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2),
                Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2)
            );

            Vector3 spawnPosition = tilePosition + randomOffset;

            // Instantiate the object and parent it to the tile
            GameObject spawnedObject = Instantiate(
                isObstacle ? obstaclePrefab : rewardPrefab,
                spawnPosition,
                Quaternion.identity,
                tile.transform // Parent to the ground tile
            );

            // Scale the spawned object
            spawnedObject.transform.localScale = isObstacle
                ? new Vector3(obstacleScale, obstacleScale, obstacleScale)
                : new Vector3(rewardScale, rewardScale, rewardScale);
        }
    }
}
