using UnityEngine;

public class PlayerVFXController : MonoBehaviour
{
    [SerializeField] private RealmManager realmManager;

    [Header("Realm Shift VFX")]
    [SerializeField] private ParticleSystem fantasyShiftVFX;
    [SerializeField] private ParticleSystem sciFiShiftVFX;

    [Header("Death VFX")]
    [SerializeField] private ParticleSystem deathVFX;

    private bool hasReceivedInitialRealm;

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
    }

    private void OnDisable()
    {
        if (realmManager == null) return;

        realmManager.OnRealmChanged -= HandleRealmChanged;
    }

    public void PlayRealmShiftVFX(RealmType realm)
    {
        ParticleSystem selected = realm == RealmType.Fantasy ? fantasyShiftVFX : sciFiShiftVFX;

        Play(selected);
    }

    public void PlayDeathVFX()
    {
        Play(deathVFX);
    }

    private void Play(ParticleSystem particle)
    {
        if (particle == null) return;

        particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        particle.Play(true);
    }

    private void HandleRealmChanged(RealmType realm)
    {
        if (!hasReceivedInitialRealm)
        {
            hasReceivedInitialRealm = true;
            return;
        }

        PlayRealmShiftVFX(realm);
    }
}