using TMPro;
using UnityEngine;

public class DistanceScoreUI : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Transform player;
    [SerializeField] private TMP_Text distanceText;

    public int CurrentDistance { get; private set; }

    private float startZ;

    private void Awake()
    {
        startZ = player.position.z;
        CurrentDistance = 0;
        UpdateText(CurrentDistance);
    }

    private void Update()
    {
        if (gameManager.CurrentState != GameState.Playing) return;

        float distance = Mathf.Max(0f, player.position.z - startZ);
        CurrentDistance = Mathf.FloorToInt(distance);

        UpdateText(CurrentDistance);
    }

    private void UpdateText(int distance)
    {
        distanceText.text = $"{distance} M";
    }
}