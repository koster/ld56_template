public class ChallengeSpider : CMSEntity
{
    public ChallengeSpider()
    {
        Define<TagPrefab>().prefab = "prefab/challenges/spider".Load<InteractiveObject>();
        Define<TagPreferSlot>().idx = 0;
        Define<TagChallengeDefinition>().goals.Add(new ChallengeGoalDefinition() { type = GoalType.EVEN });
        Define<TagChallengeDefinition>().goals.Add(new ChallengeGoalDefinition() { type = GoalType.EVEN});
        Define<TagChallengeDefinition>().goals.Add(new ChallengeGoalDefinition() { type = GoalType.EVEN });
        Define<TagChallengeDefinition>().goals.Add(new ChallengeGoalDefinition() { type = GoalType.SINK, goalValue = 20 });
        Define<TagChallengePenalty>().damage = 3;
    }
}