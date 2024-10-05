public class Level1 : CMSEntity
{
    public Level1()
    {
        Define<TagListChallenges>().all.Add(E.Id<ChallengeRock>());
    }
}