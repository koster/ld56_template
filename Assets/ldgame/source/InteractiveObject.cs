using System;
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
    public bool isClaimed;
}

public class InteractiveObject : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public SpriteRenderer spriteRenderer;
    public GameObject side;
    public SpriteRenderer spriteRendererSide;
    public SpriteRenderer spriteRendererFace;

    public Transform spinRoot;
    
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

        if (state.model.Is<TagAnimalView>(out var av))
        {
            spriteRenderer.color = av.color;
            spriteRendererSide.color = av.color;
            spriteRendererFace.sprite = av.sprite;
        }
    }

    public IEnumerator SetValue(int val)
    {
        G.audio.Play<SFX_Roll>();
        
        state.rollValue = val;
        Punch();

        if (state.rollValue > state.Sides)
        {
            G.feel.UIPunchSoft();
            yield return new WaitForSeconds(0.25f);

            state.rollValue = 1;
        }

        if (state.rollValue <= 0)
        {
            G.feel.UIPunchSoft();
            yield return new WaitForSeconds(0.25f);

            state.rollValue = state.Sides;
        }
        
        value.text = "";//state.rollValue.ToString();
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
        if (side != null)
        {
            side.SetActive(true);
            spriteRendererSide.sprite = G.main.sideEmpty;
        }

        if (state != null)
        {
            if (state.isPlayed && state.rollValue > 0 && state.rollValue <= state.Sides)
            {
                side.SetActive(true);
                spriteRendererSide.sprite = G.main.sides[state.rollValue - 1];
            }

            spriteRenderer.flipX = state.isPlayed;
        }

        if (sortingGroup != null)
            sortingGroup.sortingOrder = isMouseOver ? 9999 : order;

        if (shadow != null)
            shadow?.SetActive((zone?.isShadow ?? false) || state == null);
    }

    Tween punchTwee;
    public void Punch()
    {
        punchTwee.Kill(true);
        punchTwee=transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.2f);
    }

    public void Leave()
    {
        if (zone != null)
            zone.Release(this);
    }

    bool isMouseOver;
    
    public float Width = 1;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (G.hover_dice != null) return;
        if (G.drag_dice != null) return;
        
        isMouseOver = true;
        Width = 2.5f;
        
        if (scaleRoot)
        {
            G.hover_dice = this;
            scaleRoot.DOKill();
            scaleRoot.transform.localScale = Vector3.one * 1.25f;
        }

        if (!draggable.isDragging)
        {
            var desc = TryGetSomethingDesc();
            if (!string.IsNullOrEmpty(desc))
                G.hud.tooltip.Show(desc);
        }
    }

    string TryGetSomethingDesc()
    {
        if (state != null)
        {
            var desc = $"{GetNme()}{GetRollv()}\n\n";
            if (state.model.Is<TagRarity>(out var rr)) desc += rr.rarity.RarityToString()+"\n";
            if (state.model.Is<TagDescription>(out var td)) desc += td.loc;
            if (G.main.showEnergyValue) desc += "\n\n <color=#ff7700>Energy Value: " + GetEnergyValue()+"</color>"; 
            
            return desc;
        }

        return null;
    }

    string GetRollv()
    {
        if (state != null && state.rollValue != 0)
            return "(" + state.rollValue + ")";
        return "";
    }

    public int GetEnergyValue()
    {
        if (state.model is BasicDice)
            return 0;

        if (state.model.Is<TagOverrideEnergy>(out var oe))
            return oe.value;
            
        var tagRarity = state.model.Get<TagRarity>();
        switch (tagRarity.rarity)
        {
            case DiceRarity.COMMON: return 3;
            case DiceRarity.UNCOMMON: return 5;
            case DiceRarity.RARE: return 10;
            default: throw new ArgumentOutOfRangeException();
        }
    }

    public string GetNme()
    {
        if (state.model.Is<TagAnimalView>(out var av)) return av.name;
        return "A Tiny Creature";
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOver = false;
        G.hover_dice = null;
        Width = 1f;

        if (scaleRoot)
        {
            scaleRoot.DOKill();
            scaleRoot.DOScale(1f, 0.2f);
        }
        
        G.hud.tooltip.Hide();
    }
}