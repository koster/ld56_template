public class BasicDice : CMSEntity
{
    public BasicDice()
    {
        Define<TagPrefab>().prefab = "prefab/dice".Load<InteractiveObject>();
    }
}

public class TagPrefab : EntityComponentDefinition
{
    public InteractiveObject prefab;
}