using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    public DiceZone hand;
    public DiceZone field;

    void Start()
    {
        G.main = this;
        CMS.Init();
        
        G.OnGameReady?.Invoke();
    }

    public void TryPlayDice(InteractiveObject dice)
    {
        StartCoroutine(PlayDice(dice));
    }

    IEnumerator PlayDice(InteractiveObject dice)
    {
        field.Claim(dice);
        yield break;
    }

    public void AddDice()
    {
        var basicDice = CMS.Get<BasicDice>();
        var instance = Instantiate(basicDice.Get<TagPrefab>().prefab);
        hand.Claim(instance);
    }

    void Update()
    {
        G.ui.debug_text.text = "";
        G.ui.debug_text.text += "R-reload\n";
        G.ui.debug_text.text += "D-add dice\n";
        G.ui.debug_text.text += "I-reload with intro\n";
        
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