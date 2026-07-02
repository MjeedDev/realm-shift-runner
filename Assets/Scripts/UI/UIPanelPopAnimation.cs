using System.Collections;
using UnityEngine;

public class UIPanelPopAnimation : MonoBehaviour
{
    [SerializeField] private float startScale = 0.85f;
    [SerializeField] private float overshootScale = 1.05f;
    [SerializeField] private float popInDuration = 0.12f;
    [SerializeField] private float settleDuration = 0.08f;

    private Vector3 originalScale;
    private Coroutine popRoutine;

    private void Awake()
    {
        originalScale = transform.localScale;
    }

    private void OnEnable()
    {
        Play();
    }

    public void Play()
    {
        if (popRoutine != null)
        {
            StopCoroutine(popRoutine);
        }

        popRoutine = StartCoroutine(Pop());
    }

    private IEnumerator Pop()
    {
        transform.localScale = originalScale * startScale;

        float time = 0f;
        Vector3 overshoot = originalScale * overshootScale;

        while (time < popInDuration)
        {
            time += Time.unscaledDeltaTime;
            transform.localScale = Vector3.Lerp(originalScale * startScale, overshoot, time / popInDuration);
            yield return null;
        }

        time = 0f;

        while (time < settleDuration)
        {
            time += Time.unscaledDeltaTime;
            transform.localScale = Vector3.Lerp(overshoot, originalScale, time / settleDuration);
            yield return null;
        }

        transform.localScale = originalScale;
    }
}