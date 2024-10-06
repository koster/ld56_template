public class Level6 : CMSEntity
{
    public Level6()
    {
        Define<TagListChallenges>().all.Add(E.Id<ChallengeSpiderweb>());
    }
}