using System.Collections;
using UnityEngine;

public class RerollDice : BasicDice
{
    public RerollDice()
    {
        Define<TagTint>().color = new Color(1f, 0f, 1f, 0.5f);
        Define<TagRerollerNext>();
        Define<TagDescription>().loc = $"{TextStuff.Fudge}: Rerolls the last dice.";
    }
}

public class TagRerollerNext : EntityComponentDefinition
{
    public int val;
}

public class RerollNext : BaseInteraction, IOnPlay
{
    public IEnumerator OnPlayDice(DiceState dice)
    {
        if (dice.model.Is<TagRerollerNext>())
        {
            var last = G.main.field.LastDice();
            if (last != null)
            {
                dice.view.Punch();
                yield return new WaitForSeconds(0.25f);
                yield return G.main.Roll(last);
            }
        }
    }
}