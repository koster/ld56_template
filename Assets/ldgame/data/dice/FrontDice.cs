using System.Collections;
using UnityEngine;

public class FrontDice : BasicDice
{
    public FrontDice()
    {
        Define<TagTint>().color = new Color(.2f, 0.22f, 0.85f, .22f);
        Define<TagForceFront>();
        Define<TagDescription>().loc = $"Is always played in {TextStuff.Front}.";
        Define<TagRarity>().rarity = DiceRarity.COMMON;
    }
}

public class TagForceFront : EntityComponentDefinition
{
}

public class ForceFront : BaseInteraction, IFilterInsertPos
{
    public int OverrideIndex(int dinx, DiceZone diceZone, InteractiveObject toClaim)
    {
        if (toClaim.state.model.Is<TagForceFront>())
            return diceZone.objects.Count;
        return dinx;
    }
}