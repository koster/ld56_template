public class Level3 : CMSEntity
{
    public Level3()
    {
        Define<TagListChallenges>().all.Add(E.Id<ChallengeGoblinBig>());
    }
}