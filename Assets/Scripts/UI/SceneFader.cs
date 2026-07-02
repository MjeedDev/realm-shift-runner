using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneFader : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeInDuration = 0.75f;
    [SerializeField] private float fadeOutDuration = 0.35f;

    private bool isTransitioning;

    private void Awake()
    {
        SetAlpha(1f);
        fadeImage.gameObject.SetActive(true);
    }

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    public void FadeToScene(string sceneName)
    {
        if (isTransitioning) return;

        StartCoroutine(FadeOutAndLoad(sceneName));
    }

    private IEnumerator FadeIn()
    {
        float time = 0f;

        while (time < fadeInDuration)
        {
            time += Time.deltaTime;
            SetAlpha(Mathf.Lerp(1f, 0f, time / fadeInDuration));
            yield return null;
        }

        SetAlpha(0f);
        fadeImage.gameObject.SetActive(false);
    }

    private IEnumerator FadeOutAndLoad(string sceneName)
    {
        isTransitioning = true;
        fadeImage.gameObject.SetActive(true);

        float time = 0f;

        while (time < fadeOutDuration)
        {
            time += Time.deltaTime;
            SetAlpha(Mathf.Lerp(0f, 1f, time / fadeOutDuration));
            yield return null;
        }

        SetAlpha(1f);

        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName);

        if (loadOperation == null)
        {
            yield break;
        }

        while (!loadOperation.isDone)
        {
            yield return null;
        }
    }

    private void SetAlpha(float alpha)
    {
        Color color = fadeImage.color;
        color.a = alpha;
        fadeImage.color = color;
    }
}