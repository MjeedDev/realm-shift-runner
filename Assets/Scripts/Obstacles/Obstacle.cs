using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [Header("Obstacle Logic")]
    [SerializeField] private ObstacleRequiredAction requiredAction;
    [SerializeField] private RealmAvailability realmAvailability = RealmAvailability.BothRealms;

    public ObstacleRequiredAction RequiredAction => requiredAction;
    public RealmAvailability RealmAvailability => realmAvailability;
}