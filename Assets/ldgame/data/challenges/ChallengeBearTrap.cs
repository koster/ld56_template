public class ChallengeBearTrap : CMSEntity
{
    public ChallengeBearTrap()
    {
        Define<TagPrefab>().prefab = "prefab/challenges/bear_trap".Load<InteractiveObject>();
        Define<TagPreferSlot>().idx = 0;
        Define<TagChallengeDefinition>().goals.Add(new ChallengeGoalDefinition() { type = GoalType.VALUE, goalValue = 6 });
        Define<TagChallengeDefinition>().goals.Add(new ChallengeGoalDefinition() { type = GoalType.GREATER_THAN, goalValue = 3 });
        Define<TagChallengeDefinition>().goals.Add(new ChallengeGoalDefinition() { type = GoalType.EVEN });
        Define<TagChallengePenalty>().damage = 3;
    }
}