using UnityEngine;

public abstract class DiceBase : CMSEntity
{
    public DiceBase()
    {
        Define<TagPrefab>().prefab = "prefab/dice_view".Load<InteractiveObject>();
        Define<TagDescription>().loc = $"Regular dice";
        Define<TagSides>().sides = 6;
        Define<TagRarity>().rarity = DiceRarity.COMMON;
    }
}

public enum DiceRarity
{
    COMMON,
    UNCOMMON,
    RARE
}

public class TagRarity : EntityComponentDefinition
{
    public DiceRarity rarity;
}

public class BasicDice : DiceBase
{
    public BasicDice()
    {
    }
}

public class TagSides : EntityComponentDefinition
{
    public int sides;
}

public class TagTint : EntityComponentDefinition
{
    public Color color;
}

public class TagPrefab : EntityComponentDefinition
{
    public InteractiveObject prefab;
}