using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DraggableSmoothDamp : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public MoveableBase moveable;
    public bool isDragging = false; 

    private Camera mainCamera;

    Vector2 origin;
    Vector3 offset;
    
    private void Start()
    {
        isDragging = false;
        mainCamera = Camera.main; 
        moveable.targetPosition = transform.position; 
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        var interactiveObject = GetComponent<InteractiveObject>();
        if (interactiveObject && interactiveObject.zone && !interactiveObject.zone.canDrag)
        {
            return;
        }
        
        G.main.StartDrag(this);
        
        isDragging = true;
        
        origin = moveable.targetPosition;
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(transform.position);
        offset = transform.position - mainCamera.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, screenPosition.z));
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        G.main.StopDrag();
        
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