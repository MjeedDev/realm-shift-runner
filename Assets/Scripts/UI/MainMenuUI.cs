using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenuUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SceneFader sceneFader;

    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject howToPlayPanel;

    [Header("Scene")]
    [SerializeField] private string gameplaySceneName = "Gameplay";

    public void Play()
    {
        AudioManager.Play(AudioEventType.ButtonClick);
        AudioManager.RestoreMusic();
        sceneFader.FadeToScene(gameplaySceneName);
    }

    public void ShowHowToPlay()
    {
        AudioManager.Play(AudioEventType.ButtonClick);

        mainMenuPanel.SetActive(false);
        howToPlayPanel.SetActive(true);
    }

    public void HideHowToPlay()
    {
        AudioManager.Play(AudioEventType.ButtonClick);

        howToPlayPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void Quit()
    {
        AudioManager.Play(AudioEventType.ButtonClick);

#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}