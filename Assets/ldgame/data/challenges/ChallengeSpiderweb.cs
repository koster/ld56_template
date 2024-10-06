public class ChallengeSpiderweb : CMSEntity
{
    public ChallengeSpiderweb()
    {
        Define<TagPrefab>().prefab = "prefab/challenges/spiderwb".Load<InteractiveObject>();
        Define<TagPreferSlot>().idx = 0;
        Define<TagChallengeDefinition>().goals.Add(new ChallengeGoalDefinition() { type = GoalType.LESS_THAN, goalValue = 3 });
        Define<TagChallengeDefinition>().goals.Add(new ChallengeGoalDefinition() { type = GoalType.SINK, goalValue = 15 });
        Define<TagChallengeDefinition>().goals.Add(new ChallengeGoalDefinition() { type = GoalType.EVEN });
        Define<TagChallengeDefinition>().goals.Add(new ChallengeGoalDefinition() { type = GoalType.ODD });
        Define<TagChallengePenalty>().damage = 3;
    }
}