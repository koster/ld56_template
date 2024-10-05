using System.Collections;
using System.Collections.Generic;

public class TagListChallenges : EntityComponentDefinition
{
    public List<string> all = new List<string>();
}

public enum GoalType
{
    NONE,
    VALUE,
    GREATER_THAN,
    LESS_THAN,
    EVEN,
    ODD,
    BLOCK,
    ANY,
    SINK
}

public class ChallengeSinkCounter : BaseInteraction, IOnPutIntoGoal
{
    public IEnumerator OnGoalDice(DiceState dice, DiceHolder holder)
    {
        if (holder.spec.type == GoalType.SINK)
        {
            yield return G.main.KillDice(dice);
            holder.accumulatedValue += dice.rollValue;
        }
    }
}

public class ChallengeGoalDefinition : EntityComponentDefinition
{
    public GoalType type;
    public int goalValue;
}

public class ChallengeDefinition : EntityComponentDefinition
{
    public List<ChallengeGoalDefinition> goals = new List<ChallengeGoalDefinition>();
}


public class ChallengeRock : CMSEntity
{
    public ChallengeRock()
    {
        Define<TagPrefab>().prefab = "prefab/challenges/rock".Load<InteractiveObject>();
        Define<TagPreferSlot>().idx = 0;
        Define<ChallengeDefinition>().goals.Add(new ChallengeGoalDefinition() { type = GoalType.SINK, goalValue = 12 });
    }
}


public class ChallengeGoblinEven : CMSEntity
{
    public ChallengeGoblinEven()
    {
        Define<TagPrefab>().prefab = "prefab/challenges/goblin".Load<InteractiveObject>();
        Define<TagPreferSlot>().idx = 1;
        Define<ChallengeDefinition>().goals.Add(new ChallengeGoalDefinition() { type = GoalType.EVEN });
    }
}

public class ChallengeGoblinOdd : CMSEntity
{
    public ChallengeGoblinOdd()
    {
        Define<TagPrefab>().prefab = "prefab/challenges/goblin".Load<InteractiveObject>();
        Define<TagPreferSlot>().idx = 2;
        Define<ChallengeDefinition>().goals.Add(new ChallengeGoalDefinition() { type = GoalType.ODD });
    }
}

public class TagPreferSlot : EntityComponentDefinition
{
    public int idx;
}