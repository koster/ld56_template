using Engine.Math;
using UnityEngine;

public class EyeTracking : MonoBehaviour
{
    public float radius;

    Vector2 target;
    
    void Update()
    {
        target = Vector2.ClampMagnitude(GameMath.DirectionToMouse2DRaw(transform.position), radius);
        
        if (Vector2.Distance(transform.position, GameMath.MousePos2D()) > radius)
        {
            transform.localPosition = Vector2.Lerp(transform.localPosition, target, 0.1f);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}