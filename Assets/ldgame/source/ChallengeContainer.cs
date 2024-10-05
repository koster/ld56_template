using System.Collections.Generic;
using UnityEngine;

public class ChallengeContainer : MonoBehaviour
{
    public CMSEntity model;
    public List<DiceHolder> slots = new List<DiceHolder>();

    public bool IsComplete()
    {
        foreach (var s in slots)
        {
            if (!s.IsFilled())
                return false;
        }

        return true;
    }

    public void Load(CMSEntity challengeObject)
    {
        model = challengeObject;
        var challenge = challengeObject.Get<TagChallengeDefinition>();
        for (var index = 0; index < challenge.goals.Count; index++)
        {
            slots[index].spec = challenge.goals[index];
        }
    }
}
