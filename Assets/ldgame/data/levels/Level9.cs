public class Level9 : CMSEntity
{
    public Level9()
    {
        Define<TagListChallenges>().all.Add(E.Id<ChallengeTree>());
    }
}