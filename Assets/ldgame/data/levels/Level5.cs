public class Level5 : CMSEntity
{
    public Level5()
    {
        Define<TagListChallenges>().all.Add(E.Id<ChallengeTree>());
    }
}