using System.Collections.Generic;
using UnityEngine;

public class ObstaclePatternSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private RealmManager realmManager;
    [SerializeField] private GameObjectPool objectPool;

    [Header("Pattern Prefabs")]
    [SerializeField] private GameObject[] allPatterns;

    [Header("Balance Settings")]
    [SerializeField] private GameBalanceSO balance;
    [SerializeField] private DifficultyManager difficultyManager;

    private readonly List<GameObject> normalPatterns = new();
    private readonly List<GameObject> fantasyRealmShiftPatterns = new();
    private readonly List<GameObject> sciFiRealmShiftPatterns = new();

    private float nextSpawnZ;
    private int spawnedPatternCount;

    private void Awake()
    {
        CachePatterns();
    }

    private void Start()
    {
        nextSpawnZ = player.position.z + balance.spawnDistanceAhead;
    }

    private void Update()
    {
        if (gameManager.CurrentState != GameState.Playing)
            return;

        if (player.position.z + balance.spawnDistanceAhead < nextSpawnZ)
            return;

        SpawnPattern();
        nextSpawnZ += difficultyManager.CurrentSpawnInterval;
    }

    private void CachePatterns()
    {
        normalPatterns.Clear();
        fantasyRealmShiftPatterns.Clear();
        sciFiRealmShiftPatterns.Clear();

        foreach (GameObject patternPrefab in allPatterns)
        {
            if (patternPrefab == null)
                continue;

            if (!patternPrefab.TryGetComponent(out ObstaclePatternDefinition definition))
            {
                Debug.LogWarning($"{patternPrefab.name} has no ObstaclePatternDefinition.");
                continue;
            }

            if (definition.PatternType == ObstaclePatternType.Normal)
            {
                normalPatterns.Add(patternPrefab);
                continue;
            }

            if (definition.PatternType == ObstaclePatternType.RealmShift)
            {
                if (!patternPrefab.TryGetComponent(out Obstacle obstacle))
                {
                    Debug.LogWarning($"{patternPrefab.name} is RealmShift but has no Obstacle component.");
                    continue;
                }

                if (obstacle.RealmAvailability == RealmAvailability.FantasyOnly)
                {
                    fantasyRealmShiftPatterns.Add(patternPrefab);
                }
                else if (obstacle.RealmAvailability == RealmAvailability.SciFiOnly)
                {
                    sciFiRealmShiftPatterns.Add(patternPrefab);
                }
                else
                {
                    Debug.LogWarning($"{patternPrefab.name} is RealmShift but Obstacle RealmAvailability must be FantasyOnly or SciFiOnly.");
                }
            }
        }
    }

    private void SpawnPattern()
    {
        spawnedPatternCount++;

        bool shouldSpawnRealmShiftPattern =
            balance.realmShiftPatternEvery > 0 &&
            spawnedPatternCount % balance.realmShiftPatternEvery == 0;

        GameObject prefab = shouldSpawnRealmShiftPattern
            ? GetRealmShiftPatternForCurrentRealm()
            : GetRandomPattern(normalPatterns);

        if (prefab == null)
            return;

        objectPool.Get(
            prefab,
            new Vector3(0f, 0f, nextSpawnZ),
            Quaternion.identity
        );
    }

    private GameObject GetRealmShiftPatternForCurrentRealm()
    {
        if (realmManager.CurrentRealm == RealmType.Fantasy)
            return GetRandomPattern(fantasyRealmShiftPatterns);

        return GetRandomPattern(sciFiRealmShiftPatterns);
    }

    private GameObject GetRandomPattern(List<GameObject> patterns)
    {
        if (patterns == null || patterns.Count == 0)
            return null;

        return patterns[Random.Range(0, patterns.Count)];
    }
}