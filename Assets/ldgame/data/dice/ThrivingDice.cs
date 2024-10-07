using UnityEngine;

public class ThrivingDice : DiceBase
{
    public ThrivingDice()
    {
        Define<TagDescription>().loc = $"Is not removed from board on end turn";
        Define<TagRarity>().rarity = DiceRarity.RARE;

        Define<TagThriving>();

        Define<TagAnimalView>().name = "Rat";
        Define<TagAnimalView>().sprite = SpriteUtil.Load("animals", "rat");
        Define<TagAnimalView>().color = Color.white;
    }
}