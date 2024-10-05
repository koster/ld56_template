using System.Collections;
using UnityEngine;

public class EvenDice : BasicDice
{
    public EvenDice()
    {
        Define<TagTint>().color = Color.cyan;
        Define<TagAlwaysEven>();
        Define<TagDescription>().loc = $"{TextStuff.Fudge}: Always EVEN.";
    }
}

public class TagAlwaysEven : EntityComponentDefinition
{
}

public class AlwaysEven : BaseInteraction, IRollFilter
{
    public int OverwriteRoll(DiceState state, int roll)
    {
        if (state.model.Is<TagAlwaysEven>(out var ae))
        {
            if (roll % 2 != 0)
            {
                if (roll + 1 < state.Sides)
                    return roll + 1;
                else
                    return roll - 1;
            }
        }

        return roll;
    }
}

public interface IRollFilter
{
    public int OverwriteRoll(DiceState state, int roll);
}