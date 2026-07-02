using System.Collections;
using UnityEngine;

public class RealmGlobalVisualSwitcher : MonoBehaviour
{
    [SerializeField] private RealmManager realmManager;

    [Header("Transition")]
    [SerializeField] private float transitionDuration = 0.25f;

    [Header("Skybox")]
    [SerializeField] private Material fantasySkybox;
    [SerializeField] private Material sciFiSkybox;

    [Header("Fog")]
    [SerializeField] private bool useFog = true;
    [SerializeField] private Color fantasyFogColor = new Color(0.35f, 0.25f, 0.45f);
    [SerializeField] private Color sciFiFogColor = new Color(0.08f, 0.18f, 0.28f);
    [SerializeField] private float fantasyFogDensity = 0.01f;
    [SerializeField] private float sciFiFogDensity = 0.015f;

    [Header("Ambient Light")]
    [SerializeField] private Color fantasyAmbientColor = new Color(0.45f, 0.35f, 0.55f);
    [SerializeField] private Color sciFiAmbientColor = new Color(0.15f, 0.35f, 0.55f);

    [Header("Directional Light")]
    [SerializeField] private Light directionalLight;
    [SerializeField] private Color fantasyLightColor = new Color(1f, 0.82f, 0.55f);
    [SerializeField] private Color sciFiLightColor = new Color(0.45f, 0.75f, 1f);
    [SerializeField] private float fantasyLightIntensity = 1.1f;
    [SerializeField] private float sciFiLightIntensity = 0.9f;

    private Coroutine transitionRoutine;

    private void Awake()
    {
        if (realmManager == null)
        {
            realmManager = FindFirstObjectByType<RealmManager>();
        }
    }

    private void OnEnable()
    {
        if (realmManager == null)
        {
            realmManager = FindFirstObjectByType<RealmManager>();
        }

        if (realmManager == null) return;

        realmManager.OnRealmChanged += HandleRealmChanged;
        ApplyInstant(realmManager.CurrentRealm);
    }

    private void OnDisable()
    {
        if (realmManager == null) return;

        realmManager.OnRealmChanged -= HandleRealmChanged;
    }

    private void HandleRealmChanged(RealmType realm)
    {
        if (transitionRoutine != null)
        {
            StopCoroutine(transitionRoutine);
        }

        transitionRoutine = StartCoroutine(TransitionToRealm(realm));
    }

    private void ApplyInstant(RealmType realm)
    {
        bool isFantasy = realm == RealmType.Fantasy;

        RenderSettings.skybox = isFantasy ? fantasySkybox : sciFiSkybox;
        RenderSettings.fog = useFog;
        RenderSettings.fogColor = isFantasy ? fantasyFogColor : sciFiFogColor;
        RenderSettings.fogDensity = isFantasy ? fantasyFogDensity : sciFiFogDensity;
        RenderSettings.ambientLight = isFantasy ? fantasyAmbientColor : sciFiAmbientColor;

        if (directionalLight != null)
        {
            directionalLight.color = isFantasy ? fantasyLightColor : sciFiLightColor;
            directionalLight.intensity = isFantasy ? fantasyLightIntensity : sciFiLightIntensity;
        }

        DynamicGI.UpdateEnvironment();
    }

    private IEnumerator TransitionToRealm(RealmType realm)
    {
        bool isFantasy = realm == RealmType.Fantasy;

        RenderSettings.skybox = isFantasy ? fantasySkybox : sciFiSkybox;
        RenderSettings.fog = useFog;
        DynamicGI.UpdateEnvironment();

        Color startFogColor = RenderSettings.fogColor;
        float startFogDensity = RenderSettings.fogDensity;
        Color startAmbient = RenderSettings.ambientLight;

        Color targetFogColor = isFantasy ? fantasyFogColor : sciFiFogColor;
        float targetFogDensity = isFantasy ? fantasyFogDensity : sciFiFogDensity;
        Color targetAmbient = isFantasy ? fantasyAmbientColor : sciFiAmbientColor;

        Color startLightColor = directionalLight != null ? directionalLight.color : Color.white;
        float startLightIntensity = directionalLight != null ? directionalLight.intensity : 0f;

        Color targetLightColor = isFantasy ? fantasyLightColor : sciFiLightColor;
        float targetLightIntensity = isFantasy ? fantasyLightIntensity : sciFiLightIntensity;

        float elapsed = 0f;

        while (elapsed < transitionDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / transitionDuration);
            t = t * t * (3f - 2f * t);

            RenderSettings.fogColor = Color.Lerp(startFogColor, targetFogColor, t);
            RenderSettings.fogDensity = Mathf.Lerp(startFogDensity, targetFogDensity, t);
            RenderSettings.ambientLight = Color.Lerp(startAmbient, targetAmbient, t);

            if (directionalLight != null)
            {
                directionalLight.color = Color.Lerp(startLightColor, targetLightColor, t);
                directionalLight.intensity = Mathf.Lerp(startLightIntensity, targetLightIntensity, t);
            }

            yield return null;
        }

        ApplyInstant(realm);
        transitionRoutine = null;
    }
}