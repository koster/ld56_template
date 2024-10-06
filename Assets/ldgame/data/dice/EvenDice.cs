using System.Collections;
using Engine.Math;
using UnityEngine;

public class EvenDice : DiceBase
{
    public EvenDice()
    {
        Define<TagAlwaysEven>();
        Define<TagDescription>().loc = $"{TextStuff.Fudge}: Always EVEN.";
        Define<TagRarity>().rarity = DiceRarity.COMMON;
        
        Define<TagAnimalView>().name = "Brown Rabbit";
        Define<TagAnimalView>().sprite = SpriteUtil.Load("animals", "rabbit");
        Define<TagAnimalView>().color = "#c47906".ParseColor();
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