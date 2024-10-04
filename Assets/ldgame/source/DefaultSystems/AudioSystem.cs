using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

public enum AudioType
{
    SFX,
    Ambient,
    Music
}

public class AudioSystem : MonoBehaviour
{
    [SerializeField]
    private AudioSource ambientSource;
    [SerializeField]
    private AudioSource musicSource;

    private Dictionary<string, float> lastPlayTime = new Dictionary<string, float>();
    private List<AudioSource> audioSourcePool = new List<AudioSource>();
    private int initialPoolSize = 10;

    // Audio settings
    public bool MuteSFX { get; set; }
    public bool MuteAmbient { get; set; }
    public bool MuteMusic { get; set; }

    public float SFXVolume { get; set; } = 1.0f;
    public float AmbientVolume { get; set; } = 1.0f;
    public float MusicVolume { get; set; } = 1.0f;

    void Awake()
    {
        G.audio = this;

        musicSource = gameObject.AddComponent<AudioSource>();
        ambientSource = gameObject.AddComponent<AudioSource>();

        // Initialize the audio source pool with a predefined size
        for (int i = 0; i < initialPoolSize; i++)
        {
            var audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSourcePool.Add(audioSource);
        }
    }

    public void Play<T>() where T : CMSEntity
    {
        Play(CMS.Get<T>(Entity.Id<T>()));
    }

    public void Play(CMSEntity definition)
    {
        if (definition.Is<SFXTag>(out var sfx))
            PlaySFX(definition);
        else if (definition.Is<AmbientTag>(out var ambient))
            PlayAmbient(ambient);
        else if (definition.Is<MusicTag>(out var music))
            PlayMusic(music);
    }

    public void PlaySFX(CMSEntity sfx)
    {
        if (!MuteSFX && CanPlaySFX(sfx.id))
        {
            if (sfx.Is<SFXArray>(out var sfxarr))
            {
                var clip = sfxarr.files.GetRandom(ignoreEmpty: true);
                var audioSource = GetAvailableAudioSource();

                if (audioSource != null)
                {
                    audioSource.clip = clip;
                    audioSource.volume = SFXVolume * sfxarr.volume;
                    audioSource.Play();
                    lastPlayTime[sfx.id] = Time.time;
                    StartCoroutine(ReturnAudioSourceToPool(audioSource, clip.length));
                }
            }
        }
    }

    private AudioSource GetAvailableAudioSource()
    {
        foreach (var audioSource in audioSourcePool)
        {
            if (!audioSource.isPlaying)
            {
                return audioSource;
            }
        }

        // If no available AudioSource, create a new one and add it to the pool
        var newAudioSource = gameObject.AddComponent<AudioSource>();
        newAudioSource.playOnAwake = false;
        audioSourcePool.Add(newAudioSource);
        return newAudioSource;
    }

    private IEnumerator ReturnAudioSourceToPool(AudioSource audioSource, float delay)
    {
        yield return new WaitForSeconds(delay);
        audioSource.Stop();
        audioSource.clip = null; // Clear the clip after playing
    }

    private bool CanPlaySFX(string sfxId)
    {
        if (!lastPlayTime.ContainsKey(sfxId))
            return true;

        float lastTimePlayed = lastPlayTime[sfxId];
        float cooldown = CMS.Get<CMSEntity>(sfxId).Get<SFXTag>().Cooldown;

        return (Time.time - lastTimePlayed >= cooldown);
    }

    public void PlayAmbient(AmbientTag ambient)
    {
        if (!MuteAmbient)
        {
            ambientSource.clip = ambient.clip;
            ambientSource.loop = true;
            ambientSource.volume = AmbientVolume;
            ambientSource.Play();
        }
    }

    public void PlayMusic(MusicTag music)
    {
        if (!MuteMusic)
        {
            musicSource.clip = music.clip;
            musicSource.loop = true;
            musicSource.volume = MusicVolume;
            musicSource.Play();
        }
    }

    public void SetVolume(AudioType type, float volume)
    {
        switch (type)
        {
            case AudioType.SFX:
                SFXVolume = volume;
                break;
            case AudioType.Ambient:
                AmbientVolume = volume;
                ambientSource.volume = AmbientVolume;
                break;
            case AudioType.Music:
                MusicVolume = volume;
                musicSource.volume = volume;
                break;
        }
    }

    public void Mute(AudioType type, bool mute)
    {
        switch (type)
        {
            case AudioType.SFX:
                MuteSFX = mute;
                break;
            case AudioType.Ambient:
                MuteAmbient = mute;
                ambientSource.enabled = !mute;
                break;
            case AudioType.Music:
                MuteMusic = mute;
                musicSource.enabled = !mute;
                break;
        }
    }

    public void OnAdded()
    {
    }

    public void OnGameStarted()
    {
    }

    public void Stop(AudioType tp)
    {
        if (tp == AudioType.Ambient)
            ambientSource.Stop();
        if (tp == AudioType.Music)
            musicSource.Stop();
    }
}
