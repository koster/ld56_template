using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    public DiceZone hand;

    void Start()
    {
        G.main = this;
        CMS.Init();


        G.OnGameReady?.Invoke();
    }

    public void AddDice()
    {
        var basicDice = CMS.Get<BasicDice>();
        var instance = Instantiate(basicDice.Get<TagPrefab>().prefab);
        hand.Claim(instance);
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

        if (Input.GetKeyDown(KeyCode.D))
        {
            AddDice();
            G.feel.UIPunchSoft();
        }
    }
}