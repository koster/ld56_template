using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    void Awake()
    {
        G.main = this;
        CMS.Init();
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
