using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GroundSpawner : MonoBehaviour
{
    public GameObject groundTilePrefab; // Prefab for the ground tile/platform
    public float tileWidth = 4f; // The width of each platform tile
    public float tileSpeed = 80f; // How fast the tiles move towards the player
    public int initialTileCount = 10; // Number of tiles to spawn initially

    private Queue<GameObject> activeTiles = new Queue<GameObject>(); // Queue to store active tiles
    private float spawnZPosition; // Position where new tiles spawn
    private float despawnZPosition; // Position where tiles are despawned

    // Reference to GameTimeManager to get the speed multiplier
    public GameTimeManager gameTimeManager;

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
        // Move the ground tiles every frame at a constant speed
        MoveGround();
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
            float z = activeTiles.Peek().transform.position.z;
            RepositionTile(activeTiles.Dequeue(), z); // Remove tile from front and reposition it
        }
    }

    void SpawnTile(float pos)
    {
        GameObject newTile = Instantiate(groundTilePrefab);
        newTile.transform.position = new Vector3(0, 0, pos);
        activeTiles.Enqueue(newTile); // Add new tile to the queue
    }

    void RepositionTile(GameObject tile, float offset)
    {
        // Reposition the tile at the far end of the ground line (spawnZPosition) with offset in case of exceeding limit and gap
        tile.transform.position = new Vector3(0, 0, spawnZPosition + offset);
        activeTiles.Enqueue(tile); // Add the repositioned tile to the back of the queue
    }
}