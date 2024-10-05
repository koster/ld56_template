public class ChallengeGoblinEven : CMSEntity
{
    public ChallengeGoblinEven()
    {
        Define<TagPrefab>().prefab = "prefab/challenges/goblin".Load<InteractiveObject>();
        Define<TagPreferSlot>().idx = 1;
        Define<TagChallengeDefinition>().goals.Add(new ChallengeGoalDefinition() { type = GoalType.EVEN });
        Define<TagChallengePenalty>().damage = 3;
    }
    
}