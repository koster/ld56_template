using UnityEngine;

public class MoveableBase : ManagedBehaviour
{
    public Vector3 targetPosition;

    void OnDrawGizmos()
    {
        Gizmos.DrawSphere(targetPosition, 0.2f);
    }
}