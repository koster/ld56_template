public class ChallengeTree : CMSEntity
{
    public ChallengeTree()
    {
        Define<TagPrefab>().prefab = "prefab/challenges/tree".Load<InteractiveObject>();
        Define<TagPreferSlot>().idx = 0;
        Define<TagChallengeDefinition>().goals.Add(new ChallengeGoalDefinition() { type = GoalType.VALUE, goalValue = 1 });
        Define<TagChallengeDefinition>().goals.Add(new ChallengeGoalDefinition() { type = GoalType.VALUE, goalValue = 2 });
        Define<TagChallengeDefinition>().goals.Add(new ChallengeGoalDefinition() { type = GoalType.VALUE, goalValue = 3 });
        Define<TagChallengeDefinition>().goals.Add(new ChallengeGoalDefinition() { type = GoalType.VALUE, goalValue = 4 });
        Define<TagChallengeDefinition>().goals.Add(new ChallengeGoalDefinition() { type = GoalType.VALUE, goalValue = 5 });
        Define<TagChallengePenalty>().damage = 2;
    }
}