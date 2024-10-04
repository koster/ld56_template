using UnityEngine;
using UnityEngine.Events;

public static class GameSettings
{
    public static bool SKIP_INTRO = false;
    public static string MAIN_SCENE = "ldgame/main";
}

public static class G
{
    public static AudioSystem audio;
    public static Main main;
    public static UI ui;
    public static Savefile save;

    public static bool IsPaused;

    public static UnityAction OnGameReady;
}

public class ManagedBehaviour : MonoBehaviour
{
    void Update()
    {
        if (!G.IsPaused)
            PausableUpdate();
    }

    protected virtual void PausableUpdate()
    {
    }

    void FixedUpdate()
    {
        if (!G.IsPaused)
            PausableFixedUpdate();
    }

    protected virtual void PausableFixedUpdate()
    {
    }
}