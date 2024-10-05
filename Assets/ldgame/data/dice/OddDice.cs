using System.Collections;
using UnityEngine;

public class OddDice : BasicDice
{
    public OddDice()
    {
        Define<TagTint>().color = Color.gray;
        Define<TagAlwaysOdd>();
        Define<TagDescription>().loc = $"{TextStuff.Fudge}: Always ODD.";
    }
}

public class TagAlwaysOdd : EntityComponentDefinition
{
}

public class AlwaysOdd : BaseInteraction, IRollFilter
{
    public int OverwriteRoll(DiceState state, int roll)
    {
        if (state.model.Is<TagAlwaysOdd>(out var ae))
        {
            if (roll % 2 == 0)
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
