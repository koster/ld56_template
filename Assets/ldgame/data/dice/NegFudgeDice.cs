using UnityEngine;

public class NegFudgeDice : DiceBase
{
    public NegFudgeDice()
    {
        Define<TagFudgeDice>().delta = -1;
        Define<TagDescription>().loc = $"{TextStuff.Fudge}: Deducts 1 from the dice in {TextStuff.Front} of it!";
        Define<TagRarity>().rarity = DiceRarity.COMMON;
        
        Define<TagAnimalView>().name = "Black Cat";
        Define<TagAnimalView>().sprite = SpriteUtil.Load("animals", "cat_black");
        Define<TagAnimalView>().color = new Color(0.1f, 0.1f, 0.2f);
    }
}

public class TagDescription : EntityComponentDefinition
{
    public string loc;
}
