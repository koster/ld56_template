using UnityEngine;

public class ServicedMain : MonoBehaviour
{
    static bool isInitialized = false;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void InstantiateAutoSaveSystem()
    {
        if (!isInitialized)
        {
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
        G.camera = gameObject.AddComponent<CameraHandle>();
        G.feel = gameObject.AddComponent<Feel>();
        
        CMS.Init();

        Application.logMessageReceived += LogCallback;
    }

    void LogCallback(string condition, string stacktrace, LogType type)
    {
        // do failover for coroutines...
    }
}