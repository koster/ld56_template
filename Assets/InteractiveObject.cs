using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectState
{
    public int rollValue;
    public InteractiveObject view;
}

public class InteractiveObject : MonoBehaviour, IPointerClickHandler
{
    public ObjectState state;

    public TMP_Text value;
    
    public MoveableBase moveable;
    public DraggableSmoothDamp draggable;
    
    public DiceZone zone;

    void Start()
    {
        value.text = "";
        draggable = GetComponent<DraggableSmoothDamp>();
    }

    public void SetState(ObjectState diceState)
    {
        state = diceState;
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
}