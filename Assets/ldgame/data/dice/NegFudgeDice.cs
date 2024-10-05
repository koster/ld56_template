using System.Collections;
using UnityEngine;

public class NegFudgeDice : BasicDice
{
    public NegFudgeDice()
    {
        Define<TagTint>().color = Color.red;
        Define<TagFudgeLastDice>().delta = -1;
        Define<TagDescription>().loc = $"{TextStuff.Fudge}: Deducts 1 from the last played dice!";
    }
}

public class TagDescription : EntityComponentDefinition
{
    public string loc;
}
