public class Level4 : CMSEntity
{
    public Level4()
    {
        Define<TagListChallenges>().all.Add(E.Id<ChallengeTree>());
    }
}