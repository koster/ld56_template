using System;
using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    public MoveableBase moveable;
    public DraggableSmoothDamp draggable;

    void Start()
    {
        draggable = GetComponent<DraggableSmoothDamp>();
    }
}