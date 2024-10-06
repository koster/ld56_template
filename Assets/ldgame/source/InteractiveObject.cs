using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class DiceState
{
    public CMSEntity model;
    public int rollValue;
    public InteractiveObject view;
    public bool isPlayed;
    public bool isDead;
    public int Sides => model.Get<TagSides>().sides;
    public DiceBagState bagState;
}

public class InteractiveObject : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public SpriteRenderer spriteRenderer;
    public GameObject shadow;

    public Transform scaleRoot;

    public DiceState state;

    public TMP_Text value;

    public MoveableBase moveable;
    public DraggableSmoothDamp draggable;

    public DiceZone zone;
    
    public SortingGroup sortingGroup;

    public int order;

    void Awake()
    {
        value.text = "";
        draggable = GetComponent<DraggableSmoothDamp>();
    }

    public void SetState(DiceState diceState)
    {
        state = diceState;
        state.view = this;

        if (state.model.Is<TagTint>(out var tint))
            spriteRenderer.color = tint.color;
    }

    public IEnumerator SetValue(int val)
    {
        state.rollValue = val;

        if (state.rollValue > state.Sides)
        {
            // var delta = state.rollValue - state.Sides;
            // yield return G.main.TransferToNextDice(this, delta);
            //
            
            Punch();
            G.feel.UIPunchSoft();
            yield return new WaitForSeconds(0.25f);
            
            state.rollValue = 1;
        }
        
        value.text = state.rollValue.ToString();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (zone != null)
        {
            zone.OnClickDice?.Invoke(this);
        }
    }

    void Update()
    {
        if (state != null)
            spriteRenderer.flipX = state.isPlayed;

        if (sortingGroup != null)
            sortingGroup.sortingOrder = isMouseOver ? 9999 : order;
        
        if (shadow!=null)
            shadow?.SetActive(zone?.isShadow ?? false);
    }

    public void Punch()
    {
        transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.2f);
    }

    public void Leave()
    {
        if (zone != null)
            zone.Release(this);
    }

    bool isMouseOver;

    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseOver = true;

        if (scaleRoot)
        {
            scaleRoot.DOKill();
            scaleRoot.transform.localScale = Vector3.one * 1.25f;
        }

        var desc = TryGetSomethingDesc();
        if (!string.IsNullOrEmpty(desc))
            G.hud.tooltip.Show(desc);
    }

    string TryGetSomethingDesc()
    {
        if (state != null)
        {
            var desc = "A Tiny Creature\n\n";
            if (state.model.Is<TagRarity>(out var rr)) desc += rr.rarity.RarityToString()+"\n";
            if (state.model.Is<TagDescription>(out var td)) desc += td.loc;
            return desc;
        }

        return null;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOver = false;

        if (scaleRoot)
        {
            scaleRoot.DOKill();
            scaleRoot.DOScale(1f, 0.2f);
        }


        G.hud.tooltip.Hide();
    }
}