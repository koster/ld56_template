using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableSmoothDamp : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public MoveableBase moveable;

    private Camera mainCamera;
    private bool isDragging = false; 

    Vector2 origin;
    Vector3 offset;

    private void Start()
    {
        mainCamera = Camera.main; 
        moveable.targetPosition = transform.position; 
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true; 
        origin = moveable.targetPosition;
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(transform.position);
        offset = transform.position - mainCamera.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, screenPosition.z));
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false; 
        moveable.targetPosition = origin;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 cursorPoint = new Vector3(eventData.position.x, eventData.position.y, mainCamera.WorldToScreenPoint(transform.position).z);
        Vector3 cursorPosition = mainCamera.ScreenToWorldPoint(cursorPoint) + offset;
        cursorPosition.z = transform.position.z; 

        moveable.targetPosition = cursorPosition; 
    }
}