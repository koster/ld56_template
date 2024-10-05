using UnityEngine;

public class Min2Dice : BasicDice
{
    public Min2Dice()
    {
        Define<TagTint>().color = new Color(1f, 0.5f, 0f, 1f);
        Define<TagMinValue>().val = 2;
        Define<TagDescription>().loc = $"{TextStuff.Fudge}: Always >2.";
    }
}

public class TagMinValue : EntityComponentDefinition
{
    public int val;
}

public class MinValue : BaseInteraction, IRollFilter
{
    public int OverwriteRoll(DiceState state, int roll)
    {
        if (state.model.Is<TagMinValue>(out var ae))
        {
            if (roll < ae.val)
            {
                return ae.val;
            }
        }

        return roll;
    }
}