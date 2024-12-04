using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public GameObject rewardPrefab;
    public float spawnRate = 0.00001f;
    public Vector3 offsetPosition = new Vector3(0f, 0.5f, 0f);
    private float nextSpawnTime = 0.0f;
    public float obstacleScale = 4.0f;
    public float rewardScale = 4.0f;

    void Update()
    {

        if (Time.time > nextSpawnTime)
        {
            nextSpawnTime = Time.time + 1.0f / spawnRate;

            bool isplace = Random.value > 0.333f;
            if (isplace)
            {
                SpawnObject();
            }
        }
    }
    void SpawnObject()
    {
        bool isObstacle = Random.value > 0.5f;

        Vector3 spawnPosition = transform.position + offsetPosition;

        GameObject spawnedObject = Instantiate(isObstacle ? obstaclePrefab : rewardPrefab, spawnPosition, Quaternion.identity, transform);

        spawnedObject.transform.localScale = isObstacle ? new Vector3(obstacleScale, obstacleScale, obstacleScale) : new Vector3(rewardScale, rewardScale, rewardScale);
    }
}