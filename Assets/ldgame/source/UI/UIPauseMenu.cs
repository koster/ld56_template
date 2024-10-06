using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class UIPauseMenu : MonoBehaviour
{
    public Slider sfx;
    public Slider music;
    public Toggle postProc;

    public void Toggle()
    {
        if (gameObject.activeSelf)
            Hide();
        else
            Show();
    }

    void Show()
    {
        postProc.isOn = Camera.main.GetComponent<PostProcessLayer>().enabled;

        sfx.value = G.save.volSfx;
        music.value = G.save.volMusic;

        G.IsPaused = true;
        gameObject.SetActive(true);
    }

    void Update()
    {
        G.save.volSfx = sfx.value;
        G.save.volMusic = music.value;

        Camera.main.GetComponent<PostProcessLayer>().enabled = postProc.isOn;

        G.audio.SetVolume(AudioType.SFX, G.save.volSfx);
        G.audio.SetVolume(AudioType.Music, G.save.volMusic);
        G.audio.SetVolume(AudioType.Ambient, G.save.volMusic);
    }

    void Hide()
    {
        G.IsPaused = false;
        gameObject.SetActive(false);
    }
}