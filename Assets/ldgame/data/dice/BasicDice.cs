using UnityEngine;

public abstract class DiceBase : CMSEntity
{
    public DiceBase()
    {
        Define<TagPrefab>().prefab = "prefab/dice_view".Load<InteractiveObject>();
    }
}

public class BasicDice : DiceBase
{
    public BasicDice()
    {
    }
}

public class TagTint : EntityComponentDefinition
{
    public Color color;
}

public class TagPrefab : EntityComponentDefinition
{
    public InteractiveObject prefab;
}