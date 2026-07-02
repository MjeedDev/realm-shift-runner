using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameBalanceSO balance;

    public float CurrentForwardSpeed { get; private set; }
    public float CurrentSpawnInterval { get; private set; }

    private float elapsedTime;

    private void Awake()
    {
        CurrentForwardSpeed = balance.startForwardSpeed;
        CurrentSpawnInterval = balance.startSpawnInterval;
    }

    private void Update()
    {
        if (gameManager.CurrentState != GameState.Playing) return;

        elapsedTime += Time.deltaTime;

        CurrentForwardSpeed = Mathf.Min(balance.startForwardSpeed + elapsedTime * balance.speedIncreasePerSecond, balance.maxForwardSpeed);
        CurrentSpawnInterval = Mathf.Max(balance.startSpawnInterval - elapsedTime * balance.spawnIntervalDecreasePerSecond, balance.minSpawnInterval);
    }
}