using UnityEngine;
using UnityEngine.EventSystems;

public class UITooltipShow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [TextArea]
    public string str;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        G.hud.tooltip.Show(str);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        G.hud.tooltip.Hide();
    }
}
