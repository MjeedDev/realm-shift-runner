using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private SceneFader sceneFader;
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    [SerializeField] private string gameplaySceneName = "Gameplay";

    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject hudPanel;
    [SerializeField] private DistanceScoreUI distanceScoreUI;
    [SerializeField] private TMP_Text finalDistanceText;

    private void Awake()
    {
        gameOverPanel.SetActive(false);
        hudPanel.SetActive(true);
    }

    private void OnEnable()
    {
        gameManager.OnGameStateChanged += HandleGameStateChanged;
    }

    private void OnDisable()
    {
        gameManager.OnGameStateChanged -= HandleGameStateChanged;
    }

    public void Restart()
    {
        AudioManager.Play(AudioEventType.ButtonClick);
        AudioManager.RestoreMusic();
        sceneFader.FadeToScene(gameplaySceneName);
    }

    public void GoToMainMenu()
    {
        AudioManager.Play(AudioEventType.ButtonClick);
        AudioManager.RestoreMusic();
        sceneFader.FadeToScene(mainMenuSceneName);
    }

    private void HandleGameStateChanged(GameState state)
    {
        bool isGameOver = state == GameState.GameOver;

        gameOverPanel.SetActive(isGameOver);
        hudPanel.SetActive(!isGameOver);

        if (isGameOver)
        {
            finalDistanceText.text = $"{distanceScoreUI.CurrentDistance} M";
            AudioManager.Play(AudioEventType.Death);
            AudioManager.DuckMusic();
        }
    }
}