using System.Collections;
using UnityEngine;

public class CloneDice : BasicDice
{
    public CloneDice()
    {
        Define<TagTint>().color = new Color(.2f, 1f, 0.85f, .22f);
        Define<TagClone>();
        Define<TagDescription>().loc = $"{TextStuff.Clone}. Copies the last rolled dice.";
    }
}

public class TagClone : EntityComponentDefinition
{
}

public class Clone : BaseInteraction, IRollFilter
{
    public int OverwriteRoll(DiceState state, int roll)
    {
        if (state.model.Is<TagClone>())
        {
            var ld = G.main.field.LastDice();
            if (ld != null)
            {
                return ld.state.rollValue;
            }
        }

        return roll;
    }
}