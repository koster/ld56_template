using System;
using TMPro;
using UnityEngine;

[Serializable]
public class TargetSpecification
{
    public int value;

    public bool Matches(InteractiveObject arg0)
    {
        return arg0.state.rollValue == value;
    }
}

public class DiceHolder : MonoBehaviour
{
    public TargetSpecification spec;

    public GameObject highlight;
    public TMP_Text target;

    public int maxHold;
    
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
            zone.Claim(arg0);
        }
    }

    bool IsDiceEntrapped(InteractiveObject arg0)
    {
        if (arg0 == null)
            return false;

        var isInRange = Vector2.Distance(arg0.transform.position, transform.position) < 1f;
        if (!isInRange)
            return false;

        if (!spec.Matches(arg0))
            return false;
        
        return true;
    }

    void Update()
    {
        target.text = spec.value.ToString();
        highlight.SetActive(IsDiceEntrapped(G.drag_dice));
    }
}