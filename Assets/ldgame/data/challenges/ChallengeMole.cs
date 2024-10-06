public class ChallengeMole : CMSEntity
{
    public ChallengeMole()
    {
        Define<TagPrefab>().prefab = "prefab/challenges/mole".Load<InteractiveObject>();
        Define<TagPreferSlot>().idx = 0;
        Define<TagChallengeDefinition>().goals.Add(new ChallengeGoalDefinition() { type = GoalType.GREATER_THAN, goalValue = 1 });
        Define<TagChallengeDefinition>().goals.Add(new ChallengeGoalDefinition() { type = GoalType.GREATER_THAN, goalValue = 2 });
        Define<TagChallengeDefinition>().goals.Add(new ChallengeGoalDefinition() { type = GoalType.GREATER_THAN, goalValue = 3 });
        Define<TagChallengeDefinition>().goals.Add(new ChallengeGoalDefinition() { type = GoalType.SINK,         goalValue = 20 });
        Define<TagChallengePenalty>().damage = 2;
    }
}