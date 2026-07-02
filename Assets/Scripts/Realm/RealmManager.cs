using System;
using UnityEngine;

public class RealmManager : MonoBehaviour
{
    [SerializeField] private RealmType startingRealm = RealmType.Fantasy;

    public RealmType CurrentRealm { get; private set; }

    public event Action<RealmType> OnRealmChanged;

    private void Awake()
    {
        CurrentRealm = startingRealm;
    }

    private void Start()
    {
        OnRealmChanged?.Invoke(CurrentRealm);
    }

    public void ToggleRealm()
    {
        CurrentRealm = CurrentRealm == RealmType.Fantasy ? RealmType.SciFi : RealmType.Fantasy;

        OnRealmChanged?.Invoke(CurrentRealm);
    }
}