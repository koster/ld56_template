using System.Collections;
using UnityEngine;

public class BlockDice : DiceBase
{
    public BlockDice()
    {
        Define<TagBlock>();
        Define<TagDescription>().loc = $"{TextStuff.Block}, subtracts it's value from incoming damage.";
        Define<TagRarity>().rarity = DiceRarity.RARE;
        
        Define<TagAnimalView>().name = "Hedgehog";
        Define<TagAnimalView>().sprite = SpriteUtil.Load("animals", "hedgehog");
        Define<TagAnimalView>().color = Color.white;
    }
}

public class TagBlock : EntityComponentDefinition
{
}

public class BlockDiceInteraction : BaseInteraction, IFilterDamage
{
    public IEnumerator ProcessDamage(Main.IntOutput dmgIncoming, InteractiveObject dice)
    {
        if (dice.state.model.Is<TagBlock>(out var tag))
        {
            Debug.Log("subtract " + dmgIncoming.dmg);
            if (dmgIncoming.dmg >= dice.state.rollValue)
            {
                dmgIncoming.dmg -= dice.state.rollValue;
                yield return G.main.KillDice(dice.state);
            }
            else
            {
                yield return dice.state.view.SetValue(dice.state.rollValue - dmgIncoming.dmg);
                dmgIncoming.dmg = 0;
            }
        }
    }
}

public interface IFilterDamage
{
    public IEnumerator ProcessDamage(Main.IntOutput dmgIncoming, InteractiveObject dice);
}