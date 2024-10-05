using System.Collections;
using UnityEngine;

public class FudgeDice : BasicDice
{
    public FudgeDice()
    {
        Define<TagTint>().color = Color.yellow;
        Define<TagFudgeLastDice>().delta = 1;
        Define<TagDescription>().loc = $"{TextStuff.Fudge}: Adds 1 to the last played dice!";
    }
}

public class TagFudgeLastDice : EntityComponentDefinition
{
    public int delta;
}

public class FudgeDiceInteraction : BaseInteraction, IOnPlay
{
    public IEnumerator OnPlayDice(DiceState dice)
    {
        if (dice.model.Is<TagFudgeLastDice>(out var tfl))
        {
            var lastDice = G.main.field.LastDice();
            if (lastDice != null)
            {
                lastDice.SetValue(lastDice.state.rollValue + tfl.delta);
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