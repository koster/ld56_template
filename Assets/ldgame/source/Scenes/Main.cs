using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    void Start()
    {
        G.main = this;
        G.OnGameReady?.Invoke();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            CMS.Unload();
            SceneManager.LoadScene(0);
        }


        if (Input.GetMouseButtonDown(0))
        {
            G.audio.Play<SFX_Click>();
        }
    }
}