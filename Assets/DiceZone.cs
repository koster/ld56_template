using System;
using System.Collections.Generic;
using UnityEngine;

public class DiceZone : MonoBehaviour
{
    public List<InteractiveObject> objects = new List<InteractiveObject>();
    public float spacing = 1f;

    public void Claim(InteractiveObject toClaim)
    {
        objects.Add(toClaim);
    }

    void Update()
    {
        for (var i = 0; i < objects.Count; i++)
        {
            var offset = i * spacing - (objects.Count / 2f - 0.5f) * spacing;
            objects[i].moveable.targetPosition = transform.position + Vector3.right * offset;
        }
    }
}