using UnityEngine;

public class ObstacleRealmResolver : MonoBehaviour
{
    [SerializeField] private Obstacle obstacle;

    private RealmManager realmManager;

    private void Awake()
    {
        realmManager = FindFirstObjectByType<RealmManager>();
    }

    public bool IsActiveInCurrentRealm()
    {
        if (obstacle.RealmAvailability == RealmAvailability.BothRealms)
        {
            return true;
        }

        if (obstacle.RealmAvailability == RealmAvailability.FantasyOnly)
        {
            return realmManager.CurrentRealm == RealmType.Fantasy;
        }

        if (obstacle.RealmAvailability == RealmAvailability.SciFiOnly)
        {
            return realmManager.CurrentRealm == RealmType.SciFi;
        }

        return true;
    }
}