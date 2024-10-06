using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class TutorialMask : MonoBehaviour
{
    public TMP_Text TutorialText;
    public RectTransform mask;

    bool skip;
    
    public void SetTutorialText(string text, int pos = 0)
    {
        TutorialText.text = text;
        TutorialText.rectTransform.anchoredPosition = new Vector2(0, pos);
    }
    
    public void Show(RectTransform at)
    {
        gameObject.SetActive(true);
        
        var rect = at.rect;
        mask.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rect.width);
        mask.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, rect.height);
        mask.anchoredPosition = at.anchoredPosition;
    }

    public IEnumerator WaitForSkip()
    {
        yield return new WaitForSeconds(0.5f);
        skip = false;
        while (!skip)
        {
            yield return new WaitForEndOfFrame();
        }
        Hide();
    }
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            skip = true;
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
