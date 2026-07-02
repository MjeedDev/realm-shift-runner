using UnityEngine;

public class RealmVisualSwitcher : MonoBehaviour
{
    [Header("Visual Roots")]
    [SerializeField] private GameObject fantasyVisual;
    [SerializeField] private GameObject sciFiVisual;

    private RealmManager realmManager;

    private void Awake()
    {
        realmManager = FindFirstObjectByType<RealmManager>();
    }

    private void OnEnable()
    {
        if (realmManager == null)
        {
            realmManager = FindFirstObjectByType<RealmManager>();
        }

        if (realmManager == null) return;

        realmManager.OnRealmChanged += HandleRealmChanged;
        HandleRealmChanged(realmManager.CurrentRealm);
    }

    private void OnDisable()
    {
        if (realmManager == null) return;

        realmManager.OnRealmChanged -= HandleRealmChanged;
    }

    private void HandleRealmChanged(RealmType realm)
    {
        bool isFantasy = realm == RealmType.Fantasy;

        fantasyVisual.SetActive(isFantasy);
        sciFiVisual.SetActive(!isFantasy);
    }
}