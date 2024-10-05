using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class DiceHolder : MonoBehaviour
{
    public ChallengeGoalDefinition spec;

    public GameObject highlight;
    public TMP_Text target;

    public int maxHold;

    public int goalValue;
    public int accumulatedValue;

    public SpriteRenderer checkmark;
    
    DiceZone zone;

    void Start()
    {
        zone = GetComponent<DiceZone>();
        G.main.OnReleaseDrag += TryClaim;
    }

    void TryClaim(InteractiveObject arg0)
    {
        if (IsDiceEntrapped(arg0))
        {
            StartCoroutine(ClaimDiceIntoGoal(arg0));
        }
    }

    IEnumerator ClaimDiceIntoGoal(InteractiveObject arg0)
    {
        zone.Claim(arg0);
        var inters = G.main.interactor.FindAll<IOnPutIntoGoal>();
        foreach (var i in inters)
            yield return i.OnGoalDice(arg0.state, this);

        yield return G.main.CheckForWin();
    }

    bool IsDiceEntrapped(InteractiveObject arg0)
    {
        if (arg0 == null)
            return false;

        var isInRange = Vector2.Distance(arg0.transform.position, transform.position) < 1f;
        if (!isInRange)
            return false;

        if (!GoalMatcher.Matches(spec, arg0))
            return false;

        if (!arg0.state.isPlayed)
            return false;
        
        return true;
    }

    void Update()
    {
        RenderTarget();
        highlight.SetActive(IsDiceEntrapped(G.drag_dice));
    }

    void RenderTarget()
    {
        switch (spec.type)
        {
            case GoalType.NONE:
                target.text = "";
                break;
            case GoalType.VALUE:
                target.text = spec.goalValue.ToString();
                break;
            case GoalType.GREATER_THAN:
                target.text = ">"+spec.goalValue.ToString();
                break;
            case GoalType.LESS_THAN:
                target.text = "<"+spec.goalValue.ToString();
                break;
            case GoalType.EVEN:
                target.text = "EVEN";
                break;
            case GoalType.ODD:
                target.text = "ODD";
                break;
            case GoalType.BLOCK:
                target.text = "BLOCK";
                break;
            case GoalType.ANY:
                target.text = "ANY";
                break;
            case GoalType.SINK:
                target.text = Mathf.Max(0, spec.goalValue-accumulatedValue).ToString();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        if (IsFilled())
        {
            target.text = "";
            checkmark.gameObject.SetActive(true);
        }
    }

    public bool IsFilled()
    {
        if (spec.type == GoalType.SINK)
            return accumulatedValue >= spec.goalValue;
        
        return zone.objects.Count >= maxHold;
    }
}

public static class GoalMatcher
{
    public static bool Matches(ChallengeGoalDefinition goal, InteractiveObject obj)
    {
        if (obj == null) return false;
        if (obj.state == null) return false;
        
        switch (goal.type)
        {
            case GoalType.NONE: return true;
            
            case GoalType.VALUE: return obj.state.rollValue == goal.goalValue;
            
            case GoalType.GREATER_THAN:return obj.state.rollValue > goal.goalValue;
            
            case GoalType.LESS_THAN:return obj.state.rollValue < goal.goalValue;
            
            case GoalType.EVEN: return obj.state.rollValue % 2 == 0;
            case GoalType.ODD: return obj.state.rollValue % 2 != 0;
            
            case GoalType.BLOCK: throw new NotImplementedException(); break;
            
            case GoalType.ANY: return obj != null;
            
            case GoalType.SINK: return true;
            
            default: throw new ArgumentOutOfRangeException();
        }

        return false;
    }
}