using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GroundSpawner : MonoBehaviour
{
    public GameObject groundTilePrefab; // Prefab for the ground tile/platform
    public GameObject obstaclePrefab;
    public GameObject rewardPrefab;

    public int maxSpawnChecksReward = 3; // Max checks before a reward spawns
    public int minSpawnChecksReward = 1; // at least checks pass before reward spawns
    public int maxSpawnChecksObstacle = 5; // Max checks before an obstacle spawns
    public int minSpawnChecksObstacle = 2;

    public float baseObstacleChance = 0.3f;

    private int rewardSpawnCounter = 0; // Counter for reward spawns
    private int obstacleSpawnCounter = 0; // Counter for obstacle spawns

    public float tileWidth = 4f; // The width of each platform tile
    public float tileSpeed = 10f; // How fast the tiles move towards the player
    private int initialTileCount; // Number of tiles to spawn initially
    public float minObstacleDistance;

    private Queue<GameObject> activeTiles = new Queue<GameObject>(); // Queue to store active tiles
    public Vector3 spawnAreaSize = new Vector3(2f, 0f, 4f); // The area in which to spawn obstacles and rewards
    public int maxObstaclesPerTile = 3; // Max obstacles/rewards per tile spawn
    public int maxRewardsPerTile = 1;

    private float spawnZPosition; // Position where new tiles spawn
    private float despawnZPosition; // Position where tiles are despawned
    private float spawnAreaSizeX = 8f; // coords for spawning objects on tile
    private float spawnAreaSizeZ = 2f;

    // Reference to GameTimeManager to get the speed multiplier
    public GameTimeManager gameTimeManager;
    private Spawner_center oSpawner;

    void Start()
    {
        spawnZPosition = 92f; // Start spawning at 92 units from the player
        despawnZPosition = -(2 * tileWidth); // 2 tiles behind the player
        initialTileCount = Mathf.CeilToInt(spawnZPosition / tileWidth) + 2; // 92 units covered and 2 tiles behind
        // Spawn the initial set of tiles, starting from behind the player and up to spawnMax
        float initialPos = despawnZPosition;
        for (int i = 0; i < initialTileCount; i++)
        {
            SpawnTile(initialPos + i * tileWidth); // Spawn tiles sequentially behind the player
        }
    }

    void Update()
    {
        if (gameTimeManager.isGameOver)
        {
            return;
        }
        MoveGround();
    }

    public void incDiffulculty()
    {
        baseObstacleChance += 0.1f;
    }

    void MoveGround()
    {
        // Get the speed multiplier from the GameTimeManager
        float currentMultiplier = gameTimeManager.speedMultiplier;

        // Move the ground tiles towards the player at the specified speed (units per second)
        foreach (var tile in activeTiles)
        {
            tile.transform.Translate(Vector3.back * tileSpeed * currentMultiplier * Time.deltaTime);
        }

        // Check the front tile in the queue (first tile) to see if it needs to be repositioned
        if (activeTiles.Count > 0 && activeTiles.Peek().transform.position.z < despawnZPosition)
        {
            GameObject tile = activeTiles.Dequeue();
            float z = tile.transform.position.z;
            DespawnObjectsOnTile(tile);
            SpawnObjectOnTile(tile);
            RepositionTile(tile, z); // Remove tile from front and reposition it
        }
    }

    void SpawnTile(float pos)
    {
        GameObject newTile = Instantiate(groundTilePrefab);
        newTile.transform.position = new Vector3(0, 0, pos);
        if (activeTiles.Count < initialTileCount - 8)
        {
            SpawnObjectOnTile(newTile);
        }
        activeTiles.Enqueue(newTile); // Add new tile to the queue
    }

    void RepositionTile(GameObject tile, float offset)
    {
        // Reposition the tile at the far end of the ground line (spawnZPosition) with offset in case of exceeding limit and gap
        tile.transform.position = new Vector3(0, 0, spawnZPosition + offset);
        activeTiles.Enqueue(tile); // Add the repositioned tile to the back of the queue
    }

    // Method for spawning objects (obstacles and rewards) on the given tile
    public void SpawnObjectOnTile(GameObject tile)
    {
        // Spawn obstacles only if spawn conditions are met
        if (obstacleSpawnCounter >= minSpawnChecksObstacle && obstacleSpawnCounter <= maxSpawnChecksObstacle)
        {
            SpawnObstaclesOnTile(tile);  // Spawn obstacles
            obstacleSpawnCounter = 0;
        }
        else
            obstacleSpawnCounter++;

        // Spawn rewards only if spawn conditions are met
        if (rewardSpawnCounter >= minSpawnChecksReward && rewardSpawnCounter <= maxSpawnChecksReward)
        {
            SpawnRewardsOnTile(tile);    // Spawn rewards
            rewardSpawnCounter = 0;
        }
        else
            rewardSpawnCounter++;
    }

    // Method for spawning obstacles as children of the tile
    private void SpawnObstaclesOnTile(GameObject tile)
    {
        // Calculate the spawning area around the tile
        Vector3 tilePosition = tile.transform.position;

        // Spawn obstacles on the tile (limited by maxObstaclesPerTile)
        for (int i = 0; i < maxObstaclesPerTile; i++)
        {
            // Random chance to spawn an obstacle (50% chance for each obstacle)
            bool spawnObstacle = Random.value > baseObstacleChance;
            if (!spawnObstacle)
            {
                continue;  // If we don't spawn an obstacle, move to the next iteration
            }

            // Generate a random position within the spawn area
            Vector3 randomOffset = new Vector3(
                Random.Range(-spawnAreaSizeX / 2, spawnAreaSizeX / 2),
                1,  // Y position stays constant for simplicity (can be adjusted)
                Random.Range(-spawnAreaSizeZ / 2, spawnAreaSizeZ / 2)
            );

            Vector3 spawnPosition = tilePosition + randomOffset;

            
            // Check if the spawn position is too close to any existing obstacles on the same tile
            bool tooClose = false;
            foreach (Transform child in tile.transform)
            {
                if (child.CompareTag("Reward"))
                {
                    if (Vector3.Distance(child.position, spawnPosition) < minObstacleDistance)
                    {
                        tooClose = true;
                        break;
                    }
                } 

            }

            if (tooClose)
            {
                continue;  // Skip spawning this obstacle if it's too close to an existing one
            }
            

            // Instantiate the obstacle object and parent it to the tile
            GameObject spawnedObstacle = Instantiate(obstaclePrefab, spawnPosition, Quaternion.Euler(0, 0, 90), tile.transform);
            // Set the scale of the spawned obstacle
            spawnedObstacle.transform.localScale = new Vector3(0.5f, 1f, 1f); // Scale Y to 0.5, keeping X and Z as 1

        }
    }

    // Method for spawning rewards as children of the tile
    private void SpawnRewardsOnTile(GameObject tile)
    {
        // Calculate the spawning area around the tile
        Vector3 tilePosition = tile.transform.position;

        // Random chance to spawn a reward on the tile
        bool spawnReward = Random.value > 0.5f;
        if (spawnReward)
        {
            // Generate a random position within the spawn area for the reward
            Vector3 randomOffset = new Vector3(
                Random.Range(-spawnAreaSizeX / 2, spawnAreaSizeX / 2),
                1,  // Y position stays constant for simplicity (can be adjusted)
                Random.Range(-spawnAreaSizeZ / 2, spawnAreaSizeZ / 2)
            );

            Vector3 spawnPosition = tilePosition + randomOffset;

            // Instantiate the reward object and parent it to the tile
            GameObject spawnedReward = Instantiate(rewardPrefab, spawnPosition, Quaternion.identity, tile.transform);

        }
    }


    // A method to clear objects when they are no longer needed (only obstacles and rewards)
    public void DespawnObjectsOnTile(GameObject tile)
    {
        // Iterate over all children of the tile
        foreach (Transform child in tile.transform)
        {
            if (child != null)
            {
                // Check if the child is either an obstacle or a reward (based on prefab tags, names, or classes)
                if (child.CompareTag("Obstacle") || child.CompareTag("Reward"))
                {
                    Destroy(child.gameObject);  // Destroy the obstacle or reward
                }
            }
        }
    }
}