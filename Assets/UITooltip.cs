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


    public void Show(string text)
    {
        gameObject.SetActive(true);

        transform.DOKill(true);
        transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.2f);

        _rectTransform.anchoredPosition = G.hud.MousePos();
        label.text = text;
    }

    void Update()
    {
        if (gameObject.activeSelf)
            _rectTransform.anchoredPosition = G.hud.MousePos();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}