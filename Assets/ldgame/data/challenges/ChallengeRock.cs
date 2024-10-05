public class ChallengeRock : CMSEntity
{
    public ChallengeRock()
    {
        Define<TagPrefab>().prefab = "prefab/challenges/rock".Load<InteractiveObject>();
        Define<TagPreferSlot>().idx = 0;
        Define<TagChallengeDefinition>().goals.Add(new ChallengeGoalDefinition() { type = GoalType.SINK, goalValue = 12 });
        Define<TagChallengePenalty>().damage = 5;
    }
}