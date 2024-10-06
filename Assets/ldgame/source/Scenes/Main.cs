using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
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

    public bool HasDice(string mID)
    {
        foreach (var db in diceBag)
            if (db.id == mID)
                return true;
        return false;
    }
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

    public SpriteRenderer PlacementHint;

    public Transform HitEnergyPf;

    public Interactor interactor;

    public UnityAction<InteractiveObject> OnReleaseDrag;

    public List<ChallengeContainer> challengesActive = new List<ChallengeContainer>();
    public List<Transform> challengeSlots = new List<Transform>();

    public List<DiceBagState> diceBag = new List<DiceBagState>();
    public List<DiceBagState> discardBag = new List<DiceBagState>();

    public CMSEntity levelEntity;
    List<string> levelSeq = new List<string>()
    {
        E.Id<Level0>(),
        E.Id<Level1>(),
        E.Id<Level2>(),
        E.Id<Level3>(),
        E.Id<Level4>(),
        E.Id<Level5>(),
        E.Id<Level6>(),
        E.Id<Level7>(),
        E.Id<Level8>(),
        E.Id<Level9>(),
        E.Id<Level10>()
    };

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
        yield return G.main.Say("More creatures wanted to join the group.");
        yield return G.main.SmartWait(3f);
        G.main.AdjustSay(-1.2f);
        yield return G.main.Say("But they could only take ONE.");

        yield return SetupPicker(new List<DiceRarity>() { DiceRarity.COMMON, DiceRarity.UNCOMMON });

        yield return G.main.Say($"{pickedItem.GetNme()} was chosen.");
        yield return G.main.SmartWait(1f);

        if (levelEntity.Is<TagHard>())
        {
            G.main.AdjustSay(0f);
            yield return G.main.Say("Even more, RARER cretures desired to join.");
            yield return G.main.SmartWait(3f);

            G.main.AdjustSay(-1.2f);
            yield return G.main.Say("But still, they could only take ONE.");
            yield return SetupPicker(new List<DiceRarity> { DiceRarity.RARE });

            yield return G.main.Say($"{pickedItem.GetNme()} was chosen.");
            yield return G.main.SmartWait(1f);

            G.main.AdjustSay(0f);

            yield return G.main.Say($"The creatures were starving...");
            yield return G.main.SmartWait(3f);

            yield return G.main.Say($"One of them had to be let go.");
            yield return G.main.SmartWait(3f);

            G.main.AdjustSay(-1.2f);
            yield return G.main.Say($"Choose wisely.");

            G.main.showEnergyValue = true;

            var allDice = G.run.diceBag.ConvertAll(m => CMS.Get<DiceBase>(m.id)).ToList();
            yield return G.main.SetupPicker(allDice, showAmount: allDice.Count, addToBag: false);

            var pickedId = G.main.pickedItem.state.model.id;

            yield return G.main.picker.Clear();
            G.main.picker.Claim(CreateDice(pickedId));

            G.main.showEnergyValue = false;

            G.run.diceBag.Remove(G.run.diceBag.Find(m => m.id == pickedId));
            yield return G.main.Say($"{G.main.pickedItem.GetNme()} was left behind.");
            yield return G.main.SmartWait(3f);

            if (G.main.pickedItem.GetEnergyValue() == 0)
                yield return G.main.Say($"No Energy restored.");
            else
                yield return G.main.Say($"{G.main.pickedItem.GetEnergyValue()} Energy restored.");
            G.run.health += G.main.pickedItem.GetEnergyValue();
            if (G.run.health > G.run.maxHealth) G.run.health = G.run.maxHealth;

            yield return G.main.SmartWait(3f);
        }
    }

    public IEnumerator SetupPicker(List<DiceRarity> rarityToSuggest, int maxPick = 1, bool dontClear = false)
    {
        var allDice = CMS.GetAll<DiceBase>();

        var allRarity = allDice.FindAll(m => FitsCriteria(m, rarityToSuggest));
        allRarity.Shuffle();

        yield return SetupPicker(allRarity, maxPick, dontClear);
    }

    public float ignorePointerEnterCooldown;

    bool FitsCriteria(DiceBase m, List<DiceRarity> rarityToSuggest)
    {
        if (m.Is<TagExcludeFromReward>())
            return false;

        if (m.Is<TagCanOnlyHave1>() && G.run.HasDice(m.id))
            return false;

        return rarityToSuggest.Contains(m.Get<TagRarity>().rarity);
    }

    public IEnumerator SetupPicker(List<DiceBase> dice, int maxPick = 1, bool dontClear = false, bool addToBag = true, int showAmount = 3)
    {
        yield return picker.Clear();

        for (var i = 0; i < showAmount; i++)
            picker.Claim(CreateDice(dice[i].id));

        yield return new WaitForSeconds(0.1f);

        if (showAmount > 3)
            picker.spacing = 0.8f;
        else
            picker.spacing = 2f;

        for (var i = 0; i < maxPick; i++)
        {
            pickedItem = null;
            while (pickedItem == null) yield return new WaitForEndOfFrame();

            if (addToBag) hand.Claim(pickedItem);
        }

        if (!dontClear) yield return picker.Clear();

        if (addToBag) G.run.diceBag.Add(new DiceBagState(pickedItem.state.model.id));
    }

    public void EndTurn()
    {
        StartCoroutine(EndTurnCoroutine());
    }

    IEnumerator EndTurnCoroutine()
    {
        G.hud.DisableHud();

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

        yield return field.Clear();
        yield return hand.Clear();

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

        if (hand.objects.Count == 0)
        {
            yield return new WaitForSeconds(0.25f);
            G.hud.PunchEndTurn();
        }
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
        instance.moveable.targetPosition = instance.transform.position = Vector3.left * 7f;
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
    public bool showEnergyValue;

    void Update()
    {
        ignorePointerEnterCooldown -= Time.deltaTime;

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

        if (Input.GetKeyDown(KeyCode.W))
        {
            StartCoroutine(WinSequence());
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            G.run.level++;
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

        G.run.level++;

        if (!IsFinal()) yield return ShowPicker();

        G.fader.FadeIn();

        yield return new WaitForSeconds(1f);

        if (!IsFinal())
            SceneManager.LoadScene(GameSettings.MAIN_SCENE);
        else
            SceneManager.LoadScene("ldgame/end_screen");
    }

    bool IsFinal()
    {
        return G.run.level >= levelSeq.Count;
    }

    public class IntOutput
    {
        public int dmg;
    }

    public IEnumerator DealDamage(Vector3 origin, int dmg)
    {
        // Преобразуем позицию UI-элемента на канвасе в экранные координаты
        Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(null, G.hud.Health.transform.position);
        var pos = Camera.main.ScreenToWorldPoint(screenPos);
        var inst = Instantiate(HitEnergyPf, origin, Quaternion.identity);
        inst.transform.DOMove(pos, 0.5f);

        yield return new WaitForSeconds(0.5f);

        HitEnergyPf.GetComponent<ParticleSystem>().Stop();
        HitEnergyPf.AddComponent<Lifetime>();

        G.ui.Punch(G.hud.Health.transform);

        var outputDmg = new IntOutput() { dmg = dmg };
        var fdmg = interactor.FindAll<IFilterDamage>();
        var interactiveObjects = new List<InteractiveObject>(field.objects);
        foreach (var fDice in interactiveObjects)
        foreach (var f in fdmg)
            yield return f.ProcessDamage(outputDmg, fDice);

        G.ui.hitLight.DOFade(0.2f, 0.2f).OnComplete(() => { G.ui.hitLight.DOFade(0f, 0.2f); });

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
        say_text.transform.DOMoveY(i, 0.25f);
    }
}

public class Lifetime : MonoBehaviour
{
    public float ttl = 5f;

    void Update()
    {
        ttl -= Time.deltaTime;

        if (ttl < 0)
            Destroy(gameObject);
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
                yield return G.main.DealDamage(obstacle.transform.position, penalty.damage);
            }
        }
    }
}