using System;
using UnityEngine;

public class ServicedMain : MonoBehaviour
{
    static bool isInitialized = false;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void InstantiateAutoSaveSystem()
    {
        if (!isInitialized)
        {
            G.run = null;
            
            GameObject servicedMain = new GameObject("GameMain");
            servicedMain.AddComponent<ServicedMain>();
            DontDestroyOnLoad(servicedMain);
            isInitialized = true;
        }
    }

    void Awake()
    {
        Debug.Log("================");
        Debug.Log("entrypoint hit");
        
        // game entrypoint

        gameObject.AddComponent<Savesystem>();
        gameObject.AddComponent<AudioSystem>();
        G.fader=gameObject.AddComponent<ScreenFader>();
        
        G.camera = gameObject.AddComponent<CameraHandle>();
        G.feel = gameObject.AddComponent<Feel>();
        
        CMS.Init();
        
        G.audio.Play(CMS.Get<Sound_AmbienceForest>());

        Application.logMessageReceived += LogCallback;
    }

    void LogCallback(string condition, string stacktrace, LogType type)
    {
        // do failover for coroutines...
    }

    void OnDestroy()
    {
        isInitialized = false;
    }
}