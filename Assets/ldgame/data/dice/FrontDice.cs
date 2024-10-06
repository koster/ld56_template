using System.Collections;
using Engine.Math;
using UnityEngine;

public class FrontDice : DiceBase
{
    public FrontDice()
    {
        Define<TagTint>().color = new Color(.2f, 0.22f, 0.85f, .22f);
        Define<TagForceFront>();
        Define<TagDescription>().loc = $"Is always played in {TextStuff.Front}.";
        Define<TagRarity>().rarity = DiceRarity.COMMON;
        
        Define<TagAnimalView>().name = "Ram";
        Define<TagAnimalView>().sprite = SpriteUtil.Load("animals", "ram");
        Define<TagAnimalView>().color = "#c6ae92".ParseColor();
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