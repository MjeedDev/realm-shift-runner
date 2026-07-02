using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RealmShiftFeedbackUI : MonoBehaviour
{
    [SerializeField] private RealmManager realmManager;
    [SerializeField] private Image flashImage;

    [Header("Colors")]
    [SerializeField] private Color fantasyFlashColor = new Color(0.45f, 0.25f, 1f, 0.35f);
    [SerializeField] private Color sciFiFlashColor = new Color(0f, 0.85f, 1f, 0.35f);

    [Header("Timing")]
    [SerializeField] private float fadeOutDuration = 0.18f;

    private Coroutine flashRoutine;

    private void Awake()
    {
        SetAlpha(0f);
    }

    private void OnEnable()
    {
        realmManager.OnRealmChanged += HandleRealmChanged;
    }

    private void OnDisable()
    {
        realmManager.OnRealmChanged -= HandleRealmChanged;
    }

    private void HandleRealmChanged(RealmType realm)
    {
        Color color = realm == RealmType.Fantasy ? fantasyFlashColor : sciFiFlashColor;
        PlayFlash(color);
    }

    private void PlayFlash(Color color)
    {
        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
        }

        flashRoutine = StartCoroutine(Flash(color));
    }

    private IEnumerator Flash(Color color)
    {
        flashImage.gameObject.SetActive(true);
        flashImage.color = color;

        float startAlpha = color.a;
        float time = 0f;

        while (time < fadeOutDuration)
        {
            time += Time.unscaledDeltaTime;

            Color currentColor = color;
            currentColor.a = Mathf.Lerp(startAlpha, 0f, time / fadeOutDuration);
            flashImage.color = currentColor;

            yield return null;
        }

        SetAlpha(0f);
        flashImage.gameObject.SetActive(false);
    }

    private void SetAlpha(float alpha)
    {
        Color color = flashImage.color;
        color.a = alpha;
        flashImage.color = color;
    }
}