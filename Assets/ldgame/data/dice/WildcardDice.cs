using Engine.Math;
using UnityEngine;

public class WildcardDice : DiceBase
{
    public WildcardDice()
    {
        Define<TagWildcard>();
        Define<TagDescription>().loc = $"{TextStuff.Wildcard}: Can go into any goal!";
        Define<TagRarity>().rarity = DiceRarity.RARE;
        
        Define<TagAnimalView>().name = "Bear";
        Define<TagAnimalView>().sprite = SpriteUtil.Load("animals", "bear");
        Define<TagAnimalView>().color = "#b6a792".ParseColor();
    }
}

public class TagWildcard : EntityComponentDefinition
{
}