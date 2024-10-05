public class Level2 : CMSEntity
{
    public Level2()
    {
        Define<TagListChallenges>().all.Add(E.Id<ChallengeGoblinEven>());
        Define<TagListChallenges>().all.Add(E.Id<ChallengeGoblinOdd>());
    }
}