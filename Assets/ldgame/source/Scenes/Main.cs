using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
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
    public DiceZone storage;
    public DiceZone field;
    public DiceZone picker;

    public List<GameObject> partsOfHudToDisable;
    
    public TMP_Text say_text;
    public TMP_Text say_text_shadow;

    public Sprite sideEmpty;
    public List<Sprite> sides = new List<Sprite>();
    
    [FormerlySerializedAs("reward_up")]
    public TMP_Text RewardUp;
    [FormerlySerializedAs("reward_dn")]
    public TMP_Text ReardDn;

    public SpriteRenderer PlacementHint;

    public Interactor interactor;

    public UnityAction<InteractiveObject> OnReleaseDrag;

    public List<ChallengeContainer> challengesActive = new List<ChallengeContainer>();
    public List<Transform> challengeSlots = new List<Transform>();

    public List<DiceBagState> diceBag = new List<DiceBagState>();
    public List<DiceBagState> discardBag = new List<DiceBagState>();

    public CMSEntity levelEntity;
    List<string> levelSeq = new List<string>() { E.Id<Level0>(), E.Id<Level1>(), E.Id<Level2>(), E.Id<Level3>(), E.Id<Level4>() };

    void Awake()
    {
        interactor = new Interactor();
        interactor.Init();

        RewardUp.enabled = false;
        ReardDn.enabled = false;

        if (G.run == null)
        {
            G.run = new RunState();

            G.run.maxHealth = 15;
            G.run.health = G.run.maxHealth;

            G.run.diceBag.Add(new DiceBagState(E.Id<BasicDice>()));
            G.run.diceBag.Add(new DiceBagState(E.Id<BasicDice>()));
            G.run.diceBag.Add(new DiceBagState(E.Id<BasicDice>()));
            G.run.diceBag.Add(new DiceBagState(E.Id<FudgeDice>()));
        }

        picker.OnClickDice += OnClickPickerDice;

        G.main = this;
    }

    InteractiveObject pickedItem;

    void OnClickPickerDice(InteractiveObject arg0)
    {
        pickedItem = arg0;
    }

    public IEnumerator ShowPicker()
    {
        // RewardUp.enabled = true;
        // ReardDn.enabled = true;

        RewardUp.transform.localScale = Vector3.zero;
        ReardDn.transform.localScale = Vector3.zero;

        // RewardUp.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack);
        // ReardDn.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack);

        yield return G.main.Say("More creatures wanted to join the group.");
        yield return G.main.SmartWait(3f);
        G.main.AdjustSay(-1.2f);
        yield return G.main.Say("But they could only take ONE.");
        
        yield return SetupPicker(new List<DiceRarity>() { DiceRarity.COMMON, DiceRarity.UNCOMMON });

        yield return G.main.Say($"{pickedItem.GetNme()} was chosen.");
        yield return G.main.SmartWait(1f);
        
        if (levelEntity.Is<TagHard>())
        {
            yield return G.main.Say("Even more, RARER cretures desired to join.");
            yield return G.main.SmartWait(3f);
            yield return G.main.Say("But still, they could only take ONE.");
            yield return SetupPicker(new List<DiceRarity> { DiceRarity.RARE });

            yield return G.main.Say($"{pickedItem.GetNme()} was chosen.");
            yield return G.main.SmartWait(1f);
        }

        RewardUp.transform.DOScale(0f, 0.5f).SetEase(Ease.OutBack);
        ReardDn.transform.DOScale(0f, 0.5f).SetEase(Ease.OutBack);
    }

    public IEnumerator SetupPicker(List<DiceRarity> rarityToSuggest, int maxPick = 1, bool dontClear = false)
    {
        yield return picker.Clear();
        
        var allDice = CMS.GetAll<BasicDice>();

        var allRarity = allDice.FindAll(m => rarityToSuggest.Contains(m.Get<TagRarity>().rarity));
        allRarity.Shuffle();

        for (var i = 0; i < 3; i++)
            picker.Claim(CreateDice(allRarity[i].id));

        yield return new WaitForSeconds(0.1f);

        for (var i = 0; i < maxPick; i++)
        {
            pickedItem = null;
            while (pickedItem == null) yield return new WaitForEndOfFrame();

            hand.Claim(pickedItem);
        }

        if (!dontClear)
            yield return picker.Clear();

        G.run.diceBag.Add(new DiceBagState(pickedItem.state.model.id));
    }

    public void EndTurn()
    {
        StartCoroutine(EndTurnCoroutine());
    }

    IEnumerator EndTurnCoroutine()
    {
        G.hud.DisableHud();
        
        yield return field.Clear();

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

        yield return DrawDice();
        
        G.hud.EnableHud();
    }

    IEnumerator Start()
    {
        CMS.Init();
        
        G.hud.DisableHud();
        G.ui.DisableInput();

        G.OnGameReady?.Invoke();

        G.fader.FadeOut();
        
        if (G.run.level < levelSeq.Count)
            yield return LoadLevel(CMS.Get<CMSEntity>(levelSeq[G.run.level]));
        else
            SceneManager.LoadScene("ldgame/end_screen");

        diceBag = new List<DiceBagState>(G.run.diceBag);
        diceBag.Shuffle();

        yield return DrawDice();
        
        G.ui.EnableInput();
        G.hud.EnableHud();
    }

    IEnumerator DrawDice()
    {
        for (var i = 0; i < G.run.drawSize; i++)
        {
            if (diceBag.Count == 0)
            {
                AddDice<BasicDice>();
            }
            else
            {
                var diceBagState = diceBag.Pop();
                var diceState = AddDice(diceBagState.id);
                diceState.bagState = diceBagState;
            }

            yield return new WaitForSeconds(0.2f);
        }
    }

    public IEnumerator LoadLevel<T>() where T : CMSEntity
    {
        var entity = CMS.Get<T>();
        yield return LoadLevel(entity);
    }

    public IEnumerator LoadLevel(CMSEntity entity)
    {
        levelEntity = entity;

        if (entity.Is<TagListChallenges>(out var ls))
        {
            foreach (var challenge in ls.all)
            {
                var challengeObject = CMS.Get<CMSEntity>(challenge);
                yield return AddChallenge(challengeObject);
            }
        }

        if (levelEntity.Is<TagExecuteScript>(out var exs))
        {
            yield return exs.toExecute();

            if (challengesActive.Count == 0)
            {
                G.fader.FadeIn();
                yield return new WaitForSeconds(1f);

                G.run.level++;
                SceneManager.LoadScene(GameSettings.MAIN_SCENE);
            }
        }
    }

    public IEnumerator AddChallenge(CMSEntity challengeObject)
    {
        if (challengeObject.Is<TagPrefab>(out var pf))
        {
            var challenge = Instantiate(pf.prefab);
            var challengeContainer = challenge.GetComponent<ChallengeContainer>();
            challengeContainer.Load(challengeObject);

            var preferSlot = -1;
            var challengesActiveCount = challengesActive.Count;
            if (challengeObject.Is<TagPreferSlot>(out var pfs)) preferSlot = pfs.idx;
            if (preferSlot == -1) preferSlot = challengesActiveCount;

            var ct = challenge.transform;
            
            ct.position = challengeSlots[preferSlot].position;

            challenge.moveable.enabled = false;
            
            var offset = Vector3.up * 5;
            ct.position += offset;
            var ls = ct.localScale;
            ct.localScale = Vector3.zero;
            ct.DOScale(ls, 0.5f).SetEase(Ease.OutBack);
            ct.DOMove(ct.position - offset, 0.5f).SetEase(Ease.OutBounce);

            challengesActive.Add(challengeContainer);

            yield return new WaitForSeconds(0.5f);
        }
    }

    public void TryPlayDice(InteractiveObject dice)
    {
        StartCoroutine(PlayDice(dice));
    }

    IEnumerator PlayDice(InteractiveObject dice)
    {
        dice.state.isPlayed = true;

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
    }

    public void AddDice<T>() where T : CMSEntity
    {
        AddDice(E.Id<T>());
    }

    public DiceState AddDice(string t)
    {
        var instance = CreateDice(t);
        hand.Claim(instance);
        return instance.state;
    }

    public InteractiveObject CreateDice(string t)
    {
        var basicDice = CMS.Get<CMSEntity>(t);
        var state = new DiceState();
        state.model = basicDice;

        var instance = Instantiate(basicDice.Get<TagPrefab>().prefab);
        instance.SetState(state);
        return instance;
    }

    bool isWin;
    bool skip;

    void Update()
    {
        foreach (var poh in partsOfHudToDisable)
            poh.SetActive(G.hud.gameObject.activeSelf);
        
        if (Input.GetMouseButtonDown(0))
        {
            skip = true;
        }
        
        PlacementHintLogic();

        G.ui.debug_text.text = "";
        // G.ui.debug_text.text += "R-reload\n";
        // G.ui.debug_text.text += "D-add dice\n";
        // G.ui.debug_text.text += "I-reload with intro\n";

        if (Input.GetKeyDown(KeyCode.R))
        {
            G.run.level = 0;
            SceneManager.LoadScene(GameSettings.MAIN_SCENE);
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            G.run = null;
            SceneManager.LoadScene(0);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            StartCoroutine(DrawDice());
            G.feel.UIPunchSoft();
        }
    }

    void PlacementHintLogic()
    {
        PlacementHint.enabled = !isWin;

        if (G.drag_dice != null)
        {
            if (G.drag_dice.state.isPlayed)
                PlacementHint.color = new Color(1f, 1f, 1f, 0.05f);
            else
                PlacementHint.color = Color.white;
        }
        else
        {
            PlacementHint.color = new Color(1f, 1f, 1f, 0.05f);
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
        if (dice.bagState != null)
            discardBag.Add(dice.bagState);

        if (dice.isDead)
            yield break;

        dice.isDead = true;

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

    public IEnumerator ClearUpChallenges()
    {
        var markForKill = new List<ChallengeContainer>();

        foreach (var container in challengesActive)
            if (container.IsComplete())
                markForKill.Add(container);

        foreach (var mfk in markForKill)
            yield return ChallengeDefeated(mfk);
    }

    IEnumerator ChallengeDefeated(ChallengeContainer mfk)
    {
        mfk.transform.DOScale(0f, 0.5f);
        yield return new WaitForSeconds(0.5f);
    }

    IEnumerator WinSequence()
    {
        if (isWin)
        {
            Debug.Log("<color=red>double trigger win sequence lol</color>");
            yield break;
        }
        
        G.hud.DisableHud();

        isWin = true;
        
        G.ui.win.SetActive(true);

        yield return new WaitForSeconds(1.22f);

        G.ui.win.SetActive(false);

        // yield return storage.Clear();
        yield return field.Clear();
        yield return hand.Clear();

        yield return ShowPicker();

        G.fader.FadeIn();
        
        G.run.level++;

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(GameSettings.MAIN_SCENE);
    }

    public class IntOutput
    {
        public int dmg;
    }

    public IEnumerator DealDamage(int dmg)
    {
        G.ui.Punch(G.hud.Health.transform);
        
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

    public IEnumerator Say(string text)
    {
        StartCoroutine(Print(say_text, text));
        yield return Print(say_text_shadow, text);
    }
    
    public static IEnumerator Print(TMP_Text text, string actionDefinition, string fx = "wave")
    {
        var visibleLength = TextUtils.GetVisibleLength(actionDefinition);
        if (visibleLength == 0) yield break;
        
        for (var i = 0; i < visibleLength; i++)
        {
            text.text = $"<link={fx}>{TextUtils.CutSmart(actionDefinition, 1 + i)}</link>";
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator Unprint(TMP_Text text, string actionDefinition)
    {
        var visibleLength = TextUtils.GetVisibleLength(actionDefinition);
        if (visibleLength == 0) yield break;

        var str = "";

        for (var i = visibleLength - 1; i >= 0; i--)
        {
            str = TextUtils.CutSmart(actionDefinition, i);
            text.text = $"<link=wave>{str}</link>";
            yield return new WaitForEndOfFrame();
        }

        text.text = "";
    }

    public void HideHud()
    {
        G.hud.gameObject.SetActive(false);
    }

    public IEnumerator SmartWait(float f)
    {
        skip = false;
        while (f > 0 && !skip)
        {
            f -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    public IEnumerator Unsay()
    {
        StartCoroutine(Unprint(say_text, say_text.text));
        yield return Unprint(say_text_shadow, say_text_shadow.text);
    }

    public void AdjustSay(float i)
    {
        say_text.transform.DOMoveY(i,0.25f);
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