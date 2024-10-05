using System.Collections;
using UnityEngine;

public class FudgeDice : BasicDice
{
    public FudgeDice()
    {
        Define<TagTint>().color = Color.yellow;
        Define<TagFudgeDice>().delta = 1;
        Define<TagFudgeDice>().pos = DiceType.FRONT;
        Define<TagDescription>().loc = $"{TextStuff.Fudge}: Adds 1 to the {TextStuff.Front} dice!";
        Define<TagRarity>().rarity = DiceRarity.COMMON;
    }
}

public enum DiceType
{
    FRONT,
    LAST
}


public class TagFudgeDice : EntityComponentDefinition
{
    public int delta;
    public DiceType pos;
}

public class FudgeDiceInteraction : BaseInteraction, IOnPlay
{
    public IEnumerator OnPlayDice(DiceState dice)
    {
        if (dice.model.Is<TagFudgeDice>(out var tfl))
        {
            var lastDice = G.main.field.ResolvePos(tfl.pos);
            if (lastDice != null)
            {
                yield return lastDice.SetValue(lastDice.state.rollValue + tfl.delta);
                lastDice.Punch();
                yield return new WaitForSeconds(0.25f);
            }
        }
    }
}

public interface IOnPlay
{
    public IEnumerator OnPlayDice(DiceState dice);
}

public interface IOnPutIntoGoal
{
    public IEnumerator OnGoalDice(DiceState dice, DiceHolder holder);
}