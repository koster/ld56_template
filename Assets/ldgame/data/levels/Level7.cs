public class Level7 : CMSEntity
{
    public Level7()
    {
        Define<TagListChallenges>().all.Add(E.Id<ChallengeTree>());
    }
}