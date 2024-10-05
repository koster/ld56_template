public class ChallengeGoblinBig : CMSEntity
{
    public ChallengeGoblinBig()
    {
        Define<TagPrefab>().prefab = "prefab/challenges/goblin_big".Load<InteractiveObject>();
        Define<TagPreferSlot>().idx = 1;
        Define<TagChallengeDefinition>().goals.Add(new ChallengeGoalDefinition() { type = GoalType.VALUE, goalValue = 6});
        Define<TagChallengeDefinition>().goals.Add(new ChallengeGoalDefinition() { type = GoalType.SINK, goalValue = 20 });
        Define<TagChallengePenalty>().damage = 3;
    }
}