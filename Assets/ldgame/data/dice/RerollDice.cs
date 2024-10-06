using System.Collections;
using Engine.Math;
using UnityEngine;

public class RerollDice : DiceBase
{
    public RerollDice()
    {
        Define<TagTint>().color = new Color(1f, 0f, 1f, 0.5f);
        Define<TagRerollerNext>();
        Define<TagDescription>().loc = $"{TextStuff.Fudge}: Rerolls the last dice.";
        Define<TagRarity>().rarity = DiceRarity.UNCOMMON;
        
        Define<TagAnimalView>().name = "Frog";
        Define<TagAnimalView>().sprite = SpriteUtil.Load("animals", "frog");
        Define<TagAnimalView>().color = "#b9d161".ParseColor();
    }
}

public class TagRerollerNext : EntityComponentDefinition
{
    public int val;
}

public class RerollNext : BaseInteraction, IOnPlay
{
    public IEnumerator OnPlayDice(DiceState dice)
    {
        if (dice.model.Is<TagRerollerNext>())
        {
            var last = G.main.field.LastDice();
            if (last != null && last != dice.view)
            {
                dice.view.Punch();
                yield return new WaitForSeconds(0.25f);
                yield return G.main.Roll(last);
            }
        }
    }
}