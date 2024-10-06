using System.Collections;
using UnityEngine;

public class CloneDice : BasicDice
{
    public CloneDice()
    {
        Define<TagClone>();
        Define<TagDescription>().loc = $"{TextStuff.Clone}. Copies the last rolled dice.";
        Define<TagRarity>().rarity = DiceRarity.RARE;
        // Define<TagAnimalView>().name = "Hedgehog";
        // Define<TagAnimalView>().sprite = SpriteUtil.Load("animals", "hedgehog");
    }
}

public class TagClone : EntityComponentDefinition
{
}

public class Clone : BaseInteraction, IRollFilter
{
    public int OverwriteRoll(DiceState state, int roll)
    {
        if (state.model.Is<TagClone>())
        {
            var ld = G.main.field.LastDice();
            if (ld != null)
            {
                return ld.state.rollValue;
            }
        }

        return roll;
    }
}