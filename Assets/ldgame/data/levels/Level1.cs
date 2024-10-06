using System.Collections;

public class Level1 : CMSEntity
{
    public Level1()
    {
        Define<TagExecuteScript>().toExecute = Script;
        Define<TagListChallenges>().all.Add(E.Id<ChallengeRock>());
    }

    IEnumerator Script()
    {
        G.main.AdjustSay(2f);
        yield return G.main.Say("A big rock was blocking their path...");
        yield return G.main.SmartWait(2f);
        yield return G.main.Unsay();
    }
}