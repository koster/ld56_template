using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Main : MonoBehaviour
{
    public DiceZone hand;
    public DiceZone field;

    public Interactor interactor;

    public UnityAction<InteractiveObject> OnReleaseDrag;

    public List<ChallengeContainer> challengesActive = new List<ChallengeContainer>();
    public List<Transform> challengeSlots = new List<Transform>();

    void Awake()
    {
        interactor = new Interactor();
        interactor.Init();

        G.main = this;
    }

    void Start()
    {
        CMS.Init();

        G.OnGameReady?.Invoke();

        LoadLevel<Level1>();
    }

    public void LoadLevel<T>() where T : CMSEntity
    {
        var entity = CMS.Get<T>();
        var level = entity.Get<TagListChallenges>().all;
        foreach (var challenge in level)
        {
            var challengeObject = CMS.Get<CMSEntity>(challenge);
            AddChallenge(challengeObject);
        }
    }

    public void AddChallenge(CMSEntity challengeObject)
    {
        if (challengeObject.Is<TagPrefab>(out var pf))
        {
            var challenge = Instantiate(pf.prefab);
            var challengeContainer = challenge.GetComponent<ChallengeContainer>();
            challengeContainer.Load(challengeObject);
            challenge.transform.position = challengeSlots[challengesActive.Count].position;
            challengesActive.Add(challengeContainer);
        }
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

        yield return new WaitForSeconds(0.25f);

        var onPlayDice = interactor.FindAll<IOnPlay>();
        foreach (var onPlay in onPlayDice)
            yield return onPlay.OnPlayDice(dice.state);
    }

    public void AddDice<T>() where T : CMSEntity
    {
        AddDice(typeof(T));
    }

    public void AddDice(Type t)
    {
        var basicDice = CMS.Get<CMSEntity>(E.Id(t));
        var state = new DiceState();
        state.model = basicDice;
        
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
            if (Random.Range(0f, 1f) < 0.5f)
                AddDice<BasicDice>();
            else
                AddDice<FudgeDice>();

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

    public IEnumerator KillDice(DiceState dice)
    {
        dice.view.transform.DOScale(0f, 0.25f);
        yield return new WaitForSeconds(0.25f);
        dice.view.Leave();
        Destroy(dice.view.gameObject);
    }

    public IEnumerator CheckForWin()
    {
        foreach (var container in challengesActive)
        {
            if (!container.IsComplete())
            {
                yield break;
            }
        }

        yield return WinSequence();
    }

    IEnumerator WinSequence()
    {
        G.ui.win.SetActive(true);
        yield break;
    }
}