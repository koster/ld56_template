using System.Collections;
using UnityEngine;

public class FudgeDice : DiceBase
{
    public FudgeDice()
    {
        Define<TagTint>().color = Color.yellow;
        Define<TagFudgeDice>().delta = 1;
        Define<TagDescription>().loc = $"{TextStuff.Fudge}: Adds 1 to the dice in {TextStuff.Front} of it!";
        Define<TagRarity>().rarity = DiceRarity.COMMON;
        
        Define<TagAnimalView>().name = "Cat";
        Define<TagAnimalView>().sprite = SpriteUtil.Load("animals", "cat");
        Define<TagAnimalView>().color = Color.white;
    }
}


public class TagFudgeDice : EntityComponentDefinition
{
    public int delta;
}

public class FudgeDiceInteraction : BaseInteraction, IOnPlay
{
    public IEnumerator OnPlayDice(DiceState dice)
    {
        if (dice.model.Is<TagFudgeDice>(out var tfl))
        {
            var lastDice = G.main.field.GetNextDice(dice.view);
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