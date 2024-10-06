using UnityEngine;

public class StoreZoneInput : MonoBehaviour
{
    DiceZone zone;
    public int maxHold = 3;
    
    void Start()
    {
        zone = GetComponent<DiceZone>();
        G.main.OnReleaseDrag += OnReleaseDrag;
    }

    void OnReleaseDrag(InteractiveObject arg0)
    {
        if (arg0 == null) return;
        if (arg0.state == null) return;
        if (!arg0.state.isPlayed) return;
        if (zone.objects.Count >= maxHold) return;
        
        if (zone.IsOverlap(arg0))
        {
            zone.Claim(arg0);
        }
    }
}