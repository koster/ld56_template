using System.Collections;
using UnityEngine;

public class Plus1ToAllDice : DiceBase
{
    public Plus1ToAllDice()
    {
        Define<TagClone>();
        Define<TagDescription>().loc = $"Gives +1 to EVERY dice on the board.";
        Define<TagRarity>().rarity = DiceRarity.RARE;

        Define<TagFudgeAllDice>().delta = 1;

        Define<TagAnimalView>().name = "Panda";
        Define<TagAnimalView>().sprite = SpriteUtil.Load("animals", "panda");
        Define<TagAnimalView>().color = Color.white;
    }
}

public class TagFudgeAllDice : EntityComponentDefinition
{
    public int delta;
}

public class FudgeAllDiceInteraction : BaseInteraction, IOnPlay
{
    public IEnumerator OnPlayDice(DiceState dice)
    {
        if (dice.model.Is<TagFudgeAllDice>(out var tfl))
        {
            foreach (var lastDice in G.main.field.objects)
            {
                if (lastDice.state != dice)
                {
                    yield return lastDice.SetValue(lastDice.state.rollValue + tfl.delta);
                    lastDice.Punch();
                    yield return new WaitForSeconds(0.25f);
                }
            }
        }
    }
}