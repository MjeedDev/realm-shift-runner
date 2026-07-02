using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonFeedback : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    [SerializeField] private float pressedScale = 0.95f;
    [SerializeField] private float animationDuration = 0.08f;

    private Vector3 originalScale;
    private Coroutine scaleRoutine;

    private void Awake()
    {
        originalScale = transform.localScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        AnimateScale(originalScale * pressedScale);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        AnimateScale(originalScale);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        AnimateScale(originalScale);
    }

    private void AnimateScale(Vector3 targetScale)
    {
        if (scaleRoutine != null)
        {
            StopCoroutine(scaleRoutine);
        }

        scaleRoutine = StartCoroutine(ScaleTo(targetScale));
    }

    private IEnumerator ScaleTo(Vector3 targetScale)
    {
        Vector3 startScale = transform.localScale;
        float time = 0f;

        while (time < animationDuration)
        {
            time += Time.unscaledDeltaTime;
            transform.localScale = Vector3.Lerp(startScale, targetScale, time / animationDuration);
            yield return null;
        }

        transform.localScale = targetScale;
    }
}