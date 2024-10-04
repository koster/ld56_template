using UnityEngine;

public class ServicedMain : MonoBehaviour
{
    static bool isInitialized = false;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void InstantiateAutoSaveSystem()
    {
        if (!isInitialized)
        {
            GameObject autoSaveObject = new GameObject("GameMain");
            autoSaveObject.AddComponent<ServicedMain>();
            DontDestroyOnLoad(autoSaveObject);
            isInitialized = true;
        }
    }

    void Awake()
    {
        // game entrypoint

        gameObject.AddComponent<Savesystem>();
        gameObject.AddComponent<AudioSystem>();
        
        CMS.Init();

        Application.logMessageReceived += LogCallback;
    }

    void LogCallback(string condition, string stacktrace, LogType type)
    {
        // do failover for coroutines...
    }
}