using UnityEngine;

[CreateAssetMenu(fileName = "GameBalance", menuName = "Realm Shift Runner/Game Balance")]
public class GameBalanceSO : ScriptableObject
{
    [Header("Player Speed")]
    public float startForwardSpeed = 8f;
    public float maxForwardSpeed = 14f;
    public float speedIncreasePerSecond = 0.08f;

    [Header("Pattern Spawning")]
    public float spawnDistanceAhead = 45f;
    public float startSpawnInterval = 16f;
    public float minSpawnInterval = 10f;
    public float spawnIntervalDecreasePerSecond = 0.04f;

    [Header("Realm Shift")]
    public int realmShiftPatternEvery = 5;
}