public class Level8 : CMSEntity
{
    public Level8()
    {
        Define<TagListChallenges>().all.Add(E.Id<ChallengeHand>());
        Define<TagIsFinal>();
    }
}

public class TagIsFinal : EntityComponentDefinition
{
}