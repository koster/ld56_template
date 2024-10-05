using DG.Tweening;
using TMPro;
using UnityEngine;

public class UITooltip : MonoBehaviour
{
    public TMP_Text label;

    public void Show(string text)
    {
        gameObject.SetActive(true);

        transform.DOKill(true);
        transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.2f);
        
        GetComponent<RectTransform>().anchoredPosition = Input.mousePosition;
        label.text = text;
    }

    void Update()
    {
        if (gameObject.activeSelf)
            GetComponent<RectTransform>().anchoredPosition = Input.mousePosition;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}