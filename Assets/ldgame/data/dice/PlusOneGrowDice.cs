using System.Collections;
using UnityEngine;

public class PlusOneGrowDice : DiceBase
{
    public PlusOneGrowDice()
    {
        Define<TagDescription>().loc = $"Increases it's value by 1 each turn. \n (Is not removed on end turn)";
        Define<TagRarity>().rarity = DiceRarity.RARE;

        Define<TagGrowEachTurn>().delta = 1;
        Define<TagThriving>();
        
        Define<TagAnimalView>().name = "Wolf";
        Define<TagAnimalView>().sprite = SpriteUtil.Load("animals", "wolf");
        Define<TagAnimalView>().color = Color.white;
    }
}

public class TagThriving : EntityComponentDefinition
{
}

public class TagGrowEachTurn : EntityComponentDefinition
{
    public int delta;
}

public class GrowEachTurn : IOnEndTurnFieldDice
{
    public IEnumerator OnEndTurnInField(DiceState state)
    {
        if (state.model.Is<TagGrowEachTurn>(out var eg))
        {
            yield return state.view.SetValue(state.rollValue + eg.delta);
        }
    }
}