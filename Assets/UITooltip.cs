using DG.Tweening;
using TMPro;
using UnityEngine;

public class UITooltip : MonoBehaviour
{
    public TMP_Text label;
    RectTransform _rectTransform;

    void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }
    
    public static Vector2 MousePositionToCanvasPosition(Canvas canvas, RectTransform rectTransform)
    {
        Vector2 localPoint;
        Vector2 screenPosition = Input.mousePosition;
        Camera uiCamera = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform, 
            screenPosition, 
            uiCamera, 
            out localPoint
        );

        return localPoint;
    }
    
    public void Show(string text)
    {
        gameObject.SetActive(true);

        transform.DOKill(true);
        transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.2f);
        
        _rectTransform.anchoredPosition = MousePositionToCanvasPosition(G.hud.GetComponent<Canvas>(), G.hud.GetComponent<RectTransform>());
        label.text = text;
    }

    void Update()
    {
        if (gameObject.activeSelf)
            _rectTransform.anchoredPosition = MousePositionToCanvasPosition(G.hud.GetComponent<Canvas>(), G.hud.GetComponent<RectTransform>());
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}