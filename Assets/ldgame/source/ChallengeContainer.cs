using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChallengeContainer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public CMSEntity model;
    public List<DiceHolder> slots = new List<DiceHolder>();

    void Start()
    {
        foreach (var s in slots)
            s.owner = this;
    }

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

    public void OnPointerEnter(PointerEventData eventData)
    {
        var dsc = "A CHALLENGE\n";
        if (model.Is<TagChallengePenalty>(out var pen))
        {
            if (pen.damage > 0)
            {
                dsc += $"\n";
                dsc += $"Will take {pen.damage} {TextStuff.ENERGY} from you";
                dsc += $"\n" +
                       $"on {TextStuff.END_TURN}";
            }
        }
        G.hud.tooltip.Show(dsc);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        G.hud.tooltip.Hide();
    }
}
