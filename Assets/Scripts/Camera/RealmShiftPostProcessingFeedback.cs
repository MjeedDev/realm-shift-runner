using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class RealmShiftPostProcessingFeedback : MonoBehaviour
{
    [SerializeField] private RealmManager realmManager;
    [SerializeField] private Volume volume;

    [Header("Pulse")]
    [SerializeField] private float duration = 0.25f;
    [SerializeField] private float chromaticPeak = 0.35f;
    [SerializeField] private float bloomAddedPeak = 1f;

    private ChromaticAberration chromaticAberration;
    private Bloom bloom;
    private Coroutine pulseRoutine;
    private bool hasReceivedInitialRealm;

    private float baseChromaticIntensity;
    private float baseBloomIntensity;

    private void Awake()
    {
        if (realmManager == null)
        {
            realmManager = FindFirstObjectByType<RealmManager>();
        }

        CacheVolumeEffects();
        CacheBaseValues();
    }

    private void OnEnable()
    {
        if (realmManager == null)
        {
            realmManager = FindFirstObjectByType<RealmManager>();
        }

        if (realmManager == null)
        {
            return;
        }

        realmManager.OnRealmChanged += HandleRealmChanged;
    }

    private void OnDisable()
    {
        StopPulseAndRestore();

        if (realmManager == null) return;

        realmManager.OnRealmChanged -= HandleRealmChanged;
    }

    private void HandleRealmChanged(RealmType realm)
    {
        if (!hasReceivedInitialRealm)
        {
            hasReceivedInitialRealm = true;
            return;
        }

        StopPulseAndRestore();

        pulseRoutine = StartCoroutine(PulseRoutine());
    }

    private void CacheVolumeEffects()
    {
        chromaticAberration = null;
        bloom = null;

        if (volume == null || volume.profile == null) return;

        volume.profile.TryGet(out chromaticAberration);
        volume.profile.TryGet(out bloom);
    }

    private void CacheBaseValues()
    {
        baseChromaticIntensity = chromaticAberration != null ? chromaticAberration.intensity.value : 0f;
        baseBloomIntensity = bloom != null ? bloom.intensity.value : 0f;
    }

    private IEnumerator PulseRoutine()
    {
        CacheVolumeEffects();

        if (chromaticAberration == null && bloom == null)
        {
            yield break;
        }

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(elapsed / duration);
            float pulse = Mathf.Sin(t * Mathf.PI);

            if (chromaticAberration != null)
            {
                chromaticAberration.intensity.value = Mathf.Lerp(baseChromaticIntensity, chromaticPeak, pulse);
            }

            if (bloom != null)
            {
                bloom.intensity.value = baseBloomIntensity + bloomAddedPeak * pulse;
            }

            yield return null;
        }

        RestoreBaseValues();
        pulseRoutine = null;
    }

    private void StopPulseAndRestore()
    {
        if (pulseRoutine != null)
        {
            StopCoroutine(pulseRoutine);
            pulseRoutine = null;
        }

        RestoreBaseValues();
    }

    private void RestoreBaseValues()
    {
        if (chromaticAberration != null)
        {
            chromaticAberration.intensity.value = baseChromaticIntensity;
        }

        if (bloom != null)
        {
            bloom.intensity.value = baseBloomIntensity;
        }
    }
}