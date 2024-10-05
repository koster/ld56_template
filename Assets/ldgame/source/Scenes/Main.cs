using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class DiceBagState
{
    public string id;

    public DiceBagState(string cid)
    {
        id = cid;
    }
}

public class RunState
{
    public int level;
    public List<DiceBagState> diceBag = new List<DiceBagState>();
    public int drawSize = 3;
    public int health = 10;
    public int maxHealth = 10;
}

public class Main : MonoBehaviour
{
    public DiceZone hand;
    public DiceZone field;

    public Interactor interactor;

    public UnityAction<InteractiveObject> OnReleaseDrag;

    public List<ChallengeContainer> challengesActive = new List<ChallengeContainer>();
    public List<Transform> challengeSlots = new List<Transform>();

    public List<DiceBagState> diceBag;

    List<string> levelSeq = new List<string>() { E.Id<Level1>(), E.Id<Level2>(), E.Id<Level3>(), E.Id<Level4>() };

    void Awake()
    {
        interactor = new Interactor();
        interactor.Init();

        if (G.run == null)
        {
            G.run = new RunState();

            G.run.maxHealth = 15;
            G.run.health = G.run.maxHealth;

            G.run.diceBag.Add(new DiceBagState(E.Id<BasicDice>()));
            G.run.diceBag.Add(new DiceBagState(E.Id<BasicDice>()));
            G.run.diceBag.Add(new DiceBagState(E.Id<BasicDice>()));
            G.run.diceBag.Add(new DiceBagState(E.Id<BasicDice>()));
            G.run.diceBag.Add(new DiceBagState(E.Id<FudgeDice>()));
            G.run.diceBag.Add(new DiceBagState(E.Id<FudgeDice>()));
            G.run.diceBag.Add(new DiceBagState(E.Id<WildcardDice>()));
            G.run.diceBag.Add(new DiceBagState(E.Id<NegFudgeDice>()));
            G.run.diceBag.Add(new DiceBagState(E.Id<EvenDice>()));
            G.run.diceBag.Add(new DiceBagState(E.Id<OddDice>()));
            G.run.diceBag.Add(new DiceBagState(E.Id<Min2Dice>()));
            G.run.diceBag.Add(new DiceBagState(E.Id<RerollDice>()));
            G.run.diceBag.Add(new DiceBagState(E.Id<FrontDice>()));
            G.run.diceBag.Add(new DiceBagState(E.Id<CloneDice>()));
            G.run.diceBag.Add(new DiceBagState(E.Id<BlockDice>()));
        }

        G.main = this;
    }

    public void EndTurn()
    {
        StartCoroutine(EndTurnCoroutine());
    }

    IEnumerator EndTurnCoroutine()
    {
        var obstacles = interactor.FindAll<IOnEndTurnObstacle>();

        foreach (var ob in obstacles)
        {
            foreach (var challengeActive in challengesActive)
            {
                if (!challengeActive.IsComplete())
                {
                    yield return ob.OnEndTurn(challengeActive);
                }
            }
        }

        DrawDice();
    }

    void Start()
    {
        CMS.Init();

        G.OnGameReady?.Invoke();

        if (G.run.level < levelSeq.Count)
        {
            LoadLevel(CMS.Get<CMSEntity>(levelSeq[G.run.level]));
        }
        else
            SceneManager.LoadScene("ldgame/end_screen");

        diceBag = new List<DiceBagState>(G.run.diceBag);
        diceBag.Shuffle();
        DrawDice();
    }

    void DrawDice()
    {
        for (var i = 0; i < G.run.drawSize; i++)
        {
            if (diceBag.Count > 0)
                AddDice(diceBag.Pop().id);
        }
    }

    public void LoadLevel<T>() where T : CMSEntity
    {
        var entity = CMS.Get<T>();
        LoadLevel(entity);
    }

    public void LoadLevel(CMSEntity entity)
    {
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

            var preferSlot = -1;
            var challengesActiveCount = challengesActive.Count;
            if (challengeObject.Is<TagPreferSlot>(out var pfs))
                preferSlot = pfs.idx;
            if (preferSlot == -1)
                preferSlot = challengesActiveCount;

            challenge.transform.position = challengeSlots[preferSlot].position;
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

        yield return Roll(dice);
    }

    public IEnumerator Roll(InteractiveObject dice)
    {
        var roll = 1 + Random.Range(0, dice.state.Sides);

        var rollFill = interactor.FindAll<IRollFilter>();
        foreach (var rfilter in rollFill)
            roll = rfilter.OverwriteRoll(dice.state, roll);

        yield return dice.SetValue(roll);
        G.feel.UIPunchSoft();

        yield return new WaitForSeconds(0.25f);

        var onPlayDice = interactor.FindAll<IOnPlay>();
        foreach (var onPlay in onPlayDice)
            yield return onPlay.OnPlayDice(dice.state);

        dice.state.isPlayed = true;
    }

    public void AddDice<T>() where T : CMSEntity
    {
        AddDice(E.Id<T>());
    }

    public void AddDice(string t)
    {
        var basicDice = CMS.Get<CMSEntity>(t);
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
            G.run = null;
            SceneManager.LoadScene(GameSettings.MAIN_SCENE);
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            G.run = null;
            SceneManager.LoadScene(0);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            DrawDice();
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
        G.run.level++;
        G.ui.win.SetActive(true);

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(GameSettings.MAIN_SCENE);
    }

    public class IntOutput
    {
        public int dmg;
    }
    
    public IEnumerator DealDamage(int dmg)
    {
        var outputDmg = new IntOutput() { dmg = dmg };
        var fdmg = interactor.FindAll<IFilterDamage>();
        var interactiveObjects = new List<InteractiveObject>(field.objects);
        foreach (var fDice in interactiveObjects)
            foreach (var f in fdmg)
                yield return f.ProcessDamage(outputDmg, fDice);
        
        G.run.health -= outputDmg.dmg;
        if (G.run.health <= 0)
        {
            G.run.health = 0;
            yield return Loss();
        }
    }

    IEnumerator Loss()
    {
        G.ui.defeat.SetActive(true);
        yield return new WaitForSeconds(1f);
        G.run = null;
        SceneManager.LoadScene(GameSettings.MAIN_SCENE);
    }

    public IEnumerator TransferToNextDice(InteractiveObject obj, int delta)
    {
        var next = field.GetNextDice(obj);
        if (next != null)
            yield return next.SetValue(next.state.rollValue + delta);
    }
}

interface IOnEndTurnFieldDice
{
    public IEnumerator OnEndTurnInField(DiceState state);
}

interface IOnEndTurnObstacle
{
    public IEnumerator OnEndTurn(ChallengeContainer obstacle);
}

public class DealDamagePenalty : BaseInteraction, IOnEndTurnObstacle
{
    public IEnumerator OnEndTurn(ChallengeContainer obstacle)
    {
        if (obstacle.model.Is<TagChallengePenalty>(out var penalty))
        {
            if (penalty.damage > 0)
            {
                obstacle.GetComponent<InteractiveObject>().Punch();
                yield return G.main.DealDamage(penalty.damage);
            }
        }
    }
}