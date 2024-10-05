public class BasicDice : CMSEntity
{
    public BasicDice()
    {
        Define<TagPrefab>().prefab = "prefab/dice_view".Load<InteractiveObject>();
    }
}

public class TagPrefab : EntityComponentDefinition
{
    public InteractiveObject prefab;
}