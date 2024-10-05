using System.Collections;
using UnityEngine;

public class BlockDice : BasicDice
{
    public BlockDice()
    {
        Define<TagTint>().color = new Color(.2f, 0.22f, 0.85f, .22f);
        Define<TagBlock>();
        Define<TagDescription>().loc = $"{TextStuff.Block}, subtracts it's value from incoming damage.";
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
            if (dmgIncoming.dmg >= dice.state.rollValue)
            {
                dmgIncoming.dmg -= dice.state.rollValue;
                yield return G.main.KillDice(dice.state);
            }
            else
            {
                yield return dice.state.view.SetValue(dice.state.rollValue - dmgIncoming.dmg);
            }
        }
    }
}

public interface IFilterDamage
{
    public IEnumerator ProcessDamage(Main.IntOutput dmgIncoming, InteractiveObject dice);
}