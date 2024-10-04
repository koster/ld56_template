using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    void Start()
    {
        G.main = this;
        CMS.Init();
        
        
        G.OnGameReady?.Invoke();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(GameSettings.MAIN_SCENE);
        }
        
        if (Input.GetKeyDown(KeyCode.I))
        {
            SceneManager.LoadScene(0);
        }


        if (Input.GetMouseButtonDown(0))
        {
            G.feel.UIPunchSoft();
        }
    }
}