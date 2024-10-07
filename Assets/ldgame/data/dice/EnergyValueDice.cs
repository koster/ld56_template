using System.Collections;
using Engine.Math;
using UnityEngine;

public class EnergyValueDice : DiceBase
{
    public EnergyValueDice()
    {
        Define<TagTint>().color = new Color(.2f, 0.22f, 0.85f, .22f);
        // Define<TagForceFront>();
        Define<TagDescription>().loc = $"Has extra {TextStuff.ENERGY} value (6) when exiled.";
        Define<TagRarity>().rarity = DiceRarity.COMMON;

        Define<TagOverrideEnergy>().value = 6;
        
        Define<TagAnimalView>().name = "Ram";
        Define<TagAnimalView>().sprite = SpriteUtil.Load("animals", "ram");
        Define<TagAnimalView>().color = "#c6ae92".ParseColor();
    }
}

public class TagOverrideEnergy : EntityComponentDefinition
{
    public int value;
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