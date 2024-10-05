using UnityEngine;

public class NegFudgeDice : BasicDice
{
    public NegFudgeDice()
    {
        Define<TagTint>().color = Color.red;
        Define<TagFudgeDice>().delta = -1;
        Define<TagFudgeDice>().pos = DiceType.LAST;
        Define<TagDescription>().loc = $"{TextStuff.Fudge}: Deducts 1 from the last played dice!";
    }
}

public class TagDescription : EntityComponentDefinition
{
    public string loc;
}
