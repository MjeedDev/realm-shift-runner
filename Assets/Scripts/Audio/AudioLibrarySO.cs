using UnityEngine;

[CreateAssetMenu(menuName = "Realm Shift Runner/Audio Library")]
public class AudioLibrarySO : ScriptableObject
{
    [Header("Music")]
    public AudioClip musicLoop;

    [Header("SFX")]
    public AudioClip buttonClick;
    public AudioClip jump;
    public AudioClip slide;
    public AudioClip laneChange;
    public AudioClip realmShift;
    public AudioClip death;

    public AudioClip GetClip(AudioEventType eventType)
    {
        return eventType switch
        {
            AudioEventType.ButtonClick => buttonClick,
            AudioEventType.Jump => jump,
            AudioEventType.Slide => slide,
            AudioEventType.LaneChange => laneChange,
            AudioEventType.RealmShift => realmShift,
            AudioEventType.Death => death,
            _ => null
        };
    }
}