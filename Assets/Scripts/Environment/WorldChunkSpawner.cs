using UnityEngine;

public class WorldChunkSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObjectPool objectPool;

    [Header("World Chunks")]
    [SerializeField] private GameObject[] worldChunkPrefabs;

    [Header("Spawn Settings")]
    [SerializeField] private float chunkLength = 30f;
    [SerializeField] private int initialChunks = 4;
    [SerializeField] private float spawnAheadDistance = 60f;

    private float nextSpawnZ;

    private void Start()
    {
        for (int i = 0; i < initialChunks; i++)
        {
            SpawnChunk();
        }
    }

    private void Update()
    {
        if (gameManager.CurrentState != GameState.Playing) return;

        if (player.position.z + spawnAheadDistance >= nextSpawnZ)
        {
            SpawnChunk();
        }
    }

    private void SpawnChunk()
    {
        if (worldChunkPrefabs == null || worldChunkPrefabs.Length == 0) return;

        GameObject prefab = worldChunkPrefabs[Random.Range(0, worldChunkPrefabs.Length)];
        if (prefab == null) return;

        Vector3 spawnPosition = new Vector3(0f, 0f, nextSpawnZ);

        objectPool.Get(prefab, spawnPosition, Quaternion.identity);

        nextSpawnZ += chunkLength;
    }
}