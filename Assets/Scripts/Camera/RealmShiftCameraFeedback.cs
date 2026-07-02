using Unity.Cinemachine;
using UnityEngine;

public class RealmShiftCameraFeedback : MonoBehaviour
{
    [SerializeField] private RealmManager realmManager;
    [SerializeField] private CinemachineImpulseSource impulseSource;
    [SerializeField] private float impulseForce = 0.3f;

    private bool hasReceivedInitialRealm;

    private void Awake()
    {
        if (realmManager == null)
        {
            realmManager = FindFirstObjectByType<RealmManager>();
        }

        if (impulseSource == null)
        {
            impulseSource = GetComponent<CinemachineImpulseSource>();
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
    }

    private void OnDisable()
    {
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

        if (impulseSource != null)
        {
            impulseSource.GenerateImpulse(impulseForce);
        }
    }
}