using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "Realm Shift Runner/Player Settings")] 
public class PlayerSettingsSO : ScriptableObject
{
    [Header("Lane Movement")]
    public float laneChangeSpeed = 12f;

    [Header("Jump")]
    public float jumpHeight = 2.2f;
    public float jumpDuration = 0.45f;

    [Header("Slide")]
    public float slideDuration = 0.6f;
    public float airSlideFallSpeed = 24f;

    [Header("Death")]
    public float deathFallSpeed = 15f;
}