public class ChallengeGoblinOdd : CMSEntity
{
    public ChallengeGoblinOdd()
    {
        Define<TagPrefab>().prefab = "prefab/challenges/goblin".Load<InteractiveObject>();
        Define<TagPreferSlot>().idx = 2;
        Define<TagChallengeDefinition>().goals.Add(new ChallengeGoalDefinition() { type = GoalType.ODD });
        Define<TagChallengePenalty>().damage = 3;
    }
}