using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioLibrarySO audioLibrary;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Volume")]
    [SerializeField, Range(0f, 1f)] private float musicVolume = 0.35f;
    [SerializeField, Range(0f, 1f)] private float sfxVolume = 0.85f;

    [Header("Music Ducking")]
    [SerializeField, Range(0f, 1f)] private float duckedMusicVolumeMultiplier = 0.4f;
    [SerializeField] private float duckDuration = 0.3f;

    [Header("SFX Pitch Randomization")]
    [SerializeField] private float minRandomPitch = 0.95f;
    [SerializeField] private float maxRandomPitch = 1.05f;

    [Header("SFX Spam Protection")]
    [SerializeField] private float sameSFXCooldown = 0.03f;

    private static AudioManager instance;

    private readonly Dictionary<AudioEventType, float> lastPlayTimes = new();
    private Coroutine musicVolumeRoutine;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        ApplyVolume();
        PlayMusic();
    }

    public static void Play(AudioEventType eventType)
    {
        if (instance == null) return;

        instance.PlaySFXInternal(eventType);
    }

    public static void DuckMusic()
    {
        if (instance == null) return;

        instance.SetMusicVolume(instance.musicVolume * instance.duckedMusicVolumeMultiplier, instance.duckDuration);
    }

    public static void RestoreMusic()
    {
        if (instance == null) return;

        instance.SetMusicVolume(instance.musicVolume, instance.duckDuration);
    }

    private void PlaySFXInternal(AudioEventType eventType)
    {
        if (audioLibrary == null) return;
        if (IsOnCooldown(eventType)) return;

        AudioClip clip = audioLibrary.GetClip(eventType);

        if (clip == null || sfxSource == null) return;

        lastPlayTimes[eventType] = Time.unscaledTime;

        float originalPitch = sfxSource.pitch;
        sfxSource.pitch = ShouldRandomizePitch(eventType) ? Random.Range(minRandomPitch, maxRandomPitch) : 1f;

        sfxSource.PlayOneShot(clip, sfxVolume);
        sfxSource.pitch = originalPitch;
    }

    private bool IsOnCooldown(AudioEventType eventType)
    {
        if (!lastPlayTimes.TryGetValue(eventType, out float lastTime)) return false;

        return Time.unscaledTime - lastTime < sameSFXCooldown;
    }

    private bool ShouldRandomizePitch(AudioEventType eventType)
    {
        return eventType == AudioEventType.Jump || eventType == AudioEventType.LaneChange;
    }

    private void PlayMusic()
    {
        if (audioLibrary == null || audioLibrary.musicLoop == null || musicSource == null) return;
        if (musicSource.isPlaying) return;

        musicSource.clip = audioLibrary.musicLoop;
        musicSource.loop = true;
        musicSource.volume = musicVolume;
        musicSource.Play();
    }

    private void ApplyVolume()
    {
        if (musicSource != null)
        {
            musicSource.volume = musicVolume;
        }

        if (sfxSource != null)
        {
            sfxSource.volume = sfxVolume;
        }
    }

    private void SetMusicVolume(float targetVolume, float duration)
    {
        if (musicSource == null) return;

        if (musicVolumeRoutine != null)
        {
            StopCoroutine(musicVolumeRoutine);
        }

        musicVolumeRoutine = StartCoroutine(FadeMusicVolume(targetVolume, duration));
    }

    private IEnumerator FadeMusicVolume(float targetVolume, float duration)
    {
        float startVolume = musicSource.volume;
        float time = 0f;

        while (time < duration)
        {
            time += Time.unscaledDeltaTime;
            musicSource.volume = Mathf.Lerp(startVolume, targetVolume, time / duration);
            yield return null;
        }

        musicSource.volume = targetVolume;
    }
}