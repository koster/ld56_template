using System;
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
                AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position + Vector3.forward, SFXVolume * sfxarr.volume);
                lastPlayTime[sfx.id] = Time.time;
            }
        }
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