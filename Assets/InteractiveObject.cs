using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DiceState
{
    public CMSEntity model;
    public int rollValue;
    public InteractiveObject view;
}

public class InteractiveObject : MonoBehaviour, IPointerClickHandler
{
    public SpriteRenderer spriteRenderer;
    
    public DiceState state;

    public TMP_Text value;
    
    public MoveableBase moveable;
    public DraggableSmoothDamp draggable;
    
    public DiceZone zone;

    void Start()
    {
        value.text = "";
        draggable = GetComponent<DraggableSmoothDamp>();
    }

    public void SetState(DiceState diceState)
    {
        state = diceState;

        if (state.model.Is<TagTint>(out var tint))
            spriteRenderer.color = tint.color;
    }
    
    public void SetValue(int val)
    {
        state.rollValue = val;
        value.text = val.ToString();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (zone != null)
        {
            zone.OnClickDice?.Invoke(this);
        }
    }

    public void Punch()
    {
        transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.2f);
    }
}