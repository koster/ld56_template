using UnityEngine;

public class FieldZoneInput : MonoBehaviour
{
    DiceZone zone;
    
    void Start()
    {
        zone = GetComponent<DiceZone>();
        G.main.OnReleaseDrag += OnReleaseDrag;
    }

    void OnReleaseDrag(InteractiveObject arg0)
    {
        if (arg0 == null) return;
        if (arg0.state == null) return;
        if (arg0.state.isPlayed) return;
        
        if (zone.IsOverlap(arg0))
        {
            G.main.TryPlayDice(arg0);
        }
    }
}
