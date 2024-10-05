using System.Collections.Generic;
using UnityEngine;

public class DiceZone : MonoBehaviour
{
    public List<InteractiveObject> objects = new List<InteractiveObject>();
    public float spacing = 1f;
    public int maxHold = 999;

    public void Claim(InteractiveObject toClaim)
    {
        objects.Add(toClaim);
    }

    List<InteractiveObject> alignedSet = new List<InteractiveObject>();

    void Update()
    {
        alignedSet.Clear();
        for (var index = 0; index < objects.Count; index++)
        {
            var o = objects[index];
            // var isKindaBack = Vector2.Distance(objects[index].transform.position, GetTargetPos(index, objects)) < 0.5f;
            if (!o.draggable.isDragging/* || isKindaBack*/)
            {
                alignedSet.Add(o);
            }
        }

        for (var i = 0; i < alignedSet.Count; i++)
        {
            var targetPos = GetTargetPos(i, alignedSet);
            alignedSet[i].moveable.targetPosition = targetPos;
        }
    }

    Vector3 GetTargetPos(int i, List<InteractiveObject> setToWatch)
    {
        var offset = i * spacing - (setToWatch.Count / 2f - 0.5f) * spacing;
        var targetPos = transform.position + Vector3.right * offset;
        return targetPos;
    }
}