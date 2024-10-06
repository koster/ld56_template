using UnityEngine;

public class NegFudgeDice : BasicDice
{
    public NegFudgeDice()
    {
        Define<TagFudgeDice>().delta = -1;
        Define<TagFudgeDice>().pos = DiceType.LAST;
        Define<TagDescription>().loc = $"{TextStuff.Fudge}: Deducts 1 from the last played dice!";
        Define<TagRarity>().rarity = DiceRarity.COMMON;
        
        Define<TagAnimalView>().name = "Black Cat";
        Define<TagAnimalView>().sprite = SpriteUtil.Load("animals", "cat_black");
        Define<TagAnimalView>().color = new Color(0.1f, 0.1f, 0.1f);
    }
}

public class TagDescription : EntityComponentDefinition
{
    public string loc;
}
