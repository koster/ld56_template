using System.Collections.Generic;
using UnityEngine;

public class ChallengeContainer : MonoBehaviour
{
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
        var challenge = challengeObject.Get<ChallengeDefinition>();
        for (var index = 0; index < challenge.goals.Count; index++)
        {
            slots[index].spec = challenge.goals[index];
        }
    }
}
