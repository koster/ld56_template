using UnityEngine;

public class MoveableSmoothDamp : MoveableBase
{
    private Vector3 velocity; 
    public float smoothTime = 0.3F; 
    public float maxVelocity = 10f; 
    private Vector3 currentVelocity;

    protected override void PausableUpdate()
    {
        MoveXY();
    }

    protected void MoveXY()
    {
        if (Vector3.Distance(transform.position, targetPosition) > 0.01f || velocity.magnitude > 0.01f)
        {
            Vector3 newPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothTime, maxVelocity, Time.deltaTime);
            velocity = (newPosition - (Vector3)transform.position) / Time.deltaTime;

            if (velocity.sqrMagnitude > maxVelocity * maxVelocity)
            {
                velocity = velocity.normalized * maxVelocity;
            }

            transform.position = newPosition + velocity * Time.deltaTime;
            if (Vector3.Distance((Vector3)transform.position, targetPosition) < 0.01f && velocity.magnitude < 0.01f)
            {
                transform.position = new Vector3(targetPosition.x, targetPosition.y, transform.position.z);
                velocity = Vector3.zero;
            }
        }
    }
}