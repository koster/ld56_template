public class ChallengeMole : CMSEntity
{
    public ChallengeMole()
    {
        Define<TagPrefab>().prefab = "prefab/challenges/mole".Load<InteractiveObject>();
        Define<TagPreferSlot>().idx = 0;
        Define<TagChallengeDefinition>().goals.Add(new ChallengeGoalDefinition() { type = GoalType.VALUE, goalValue = 1 });
        Define<TagChallengeDefinition>().goals.Add(new ChallengeGoalDefinition() { type = GoalType.VALUE, goalValue = 1 });
        Define<TagChallengeDefinition>().goals.Add(new ChallengeGoalDefinition() { type = GoalType.VALUE, goalValue = 1 });
        Define<TagChallengeDefinition>().goals.Add(new ChallengeGoalDefinition() { type = GoalType.SINK, goalValue = 15 });
        Define<TagChallengePenalty>().damage = 2;
    }
}