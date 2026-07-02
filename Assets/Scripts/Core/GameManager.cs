using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameState CurrentState { get; private set; }

    public event Action<GameState> OnGameStateChanged;

    private void Awake()
    {
        CurrentState = GameState.Playing;
    }

    public void GameOver()
    {
        if (CurrentState == GameState.GameOver) return;

        CurrentState = GameState.GameOver;
        OnGameStateChanged?.Invoke(CurrentState);
    }
}