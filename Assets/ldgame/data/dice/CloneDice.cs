using System.Collections;
using Engine.Math;
using UnityEngine;

public class CloneDice : DiceBase
{
    public CloneDice()
    {
        Define<TagClone>();
        Define<TagDescription>().loc = $"{TextStuff.Clone}. Copies the last rolled dice.";
        Define<TagRarity>().rarity = DiceRarity.RARE;
     
        Define<TagAnimalView>().name = "Monkey";
        Define<TagAnimalView>().sprite = SpriteUtil.Load("animals", "monkey");
        Define<TagAnimalView>().color = "#a98d68".ParseColor();
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