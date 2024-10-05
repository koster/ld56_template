using TMPro;
using UnityEngine;

public class PenaltyView : MonoBehaviour
{
    public ChallengeContainer container;
    public TMP_Text text;
    
    public void Update()
    {
        if (container.model.Is<TagChallengePenalty>(out var tp))
        {
            text.text = "DMG " + tp.damage;
        }
        else
        {
            text.text = "";
        }
    }
}