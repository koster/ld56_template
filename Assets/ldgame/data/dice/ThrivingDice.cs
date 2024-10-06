using UnityEngine;

public class ThrivingDice : CMSEntity
{
    public ThrivingDice()
    {
        Define<TagDescription>().loc = $"Is not removed on end turn";
        Define<TagRarity>().rarity = DiceRarity.RARE;

        Define<TagGrowEachTurn>().delta = 1;
        Define<TagThriving>();

        Define<TagAnimalView>().name = "Rat";
        Define<TagAnimalView>().sprite = SpriteUtil.Load("animals", "rat");
        Define<TagAnimalView>().color = Color.white;
    }
}