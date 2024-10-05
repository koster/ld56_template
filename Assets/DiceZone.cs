using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DiceZone : MonoBehaviour
{
    public List<InteractiveObject> objects = new List<InteractiveObject>();
    public float spacing = 1f;
    public int maxHold = 999;
    public UnityAction<InteractiveObject> OnClickDice;

    public void Claim(InteractiveObject toClaim)
    {
        if (toClaim.zone != null)
            toClaim.zone.Release(toClaim);

        toClaim.zone = this;
        objects.Add(toClaim);
    }

    public void Release(InteractiveObject toClaim)
    {
        if (objects.Contains(toClaim))
            objects.Remove(toClaim);
    }

    List<InteractiveObject> alignedSet = new List<InteractiveObject>();

    void Update()
    {
        alignedSet.Clear();
        for (var index = 0; index < objects.Count; index++)
        {
            var o = objects[index];
            // var isKindaBack = Vector2.Distance(objects[index].transform.position, GetTargetPos(index, objects)) < 0.5f;
            if (!o.draggable.isDragging /* || isKindaBack*/)
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

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.5f, 0, 0, 0.25f);
        Gizmos.DrawCube(transform.position, new Vector3(1, 1, 1));
        Gizmos.DrawWireCube(transform.position, new Vector3(1, 1, 1));
    }
}