using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractiveObject : MonoBehaviour, IPointerClickHandler
{
    public MoveableBase moveable;
    public DraggableSmoothDamp draggable;
    
    public DiceZone zone;

    void Start()
    {
        draggable = GetComponent<DraggableSmoothDamp>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (zone != null)
        {
            zone.OnClickDice?.Invoke(this);
        }
    }
}