public class Level10 : CMSEntity
{
    public Level10()
    {
        Define<TagListChallenges>().all.Add(E.Id<ChallengeTree>());
    }
}