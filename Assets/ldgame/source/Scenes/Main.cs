using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Main : MonoBehaviour
{
    public DiceZone hand;
    public DiceZone field;

    public UnityAction<InteractiveObject> OnReleaseDrag;

    void Awake()
    {
        G.main = this;
    }

    void Start()
    {
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
        
        yield return new WaitForSeconds(0.25f);

        var roll = 1 + Random.Range(0, 6);
        dice.SetValue(roll);
        G.feel.UIPunchSoft();
        
        yield break;
    }

    public void AddDice()
    {
        var basicDice = CMS.Get<BasicDice>();
        var state = new ObjectState();
        var instance = Instantiate(basicDice.Get<TagPrefab>().prefab);
        instance.SetState(state);
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

    public void StartDrag(DraggableSmoothDamp draggableSmoothDamp)
    {
        G.drag_dice = draggableSmoothDamp.GetComponent<InteractiveObject>();
    }

    public void StopDrag()
    {
        OnReleaseDrag?.Invoke(G.drag_dice);
        G.drag_dice = null;
    }
}