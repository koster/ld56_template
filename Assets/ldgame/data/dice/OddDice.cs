using System.Collections;
using UnityEngine;

public class OddDice : BasicDice
{
    public OddDice()
    {
        Define<TagAlwaysOdd>();
        Define<TagDescription>().loc = $"{TextStuff.Fudge}: Always ODD.";
        Define<TagRarity>().rarity = DiceRarity.COMMON;
        
        Define<TagAnimalView>().name = "Grey Rabbit";
        Define<TagAnimalView>().sprite = SpriteUtil.Load("animals", "rabbit");
        Define<TagAnimalView>().color = Color.grey;
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
