using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class TutorialMask : MonoBehaviour
{
    public TMP_Text TutorialText;
    public RectTransform mask;
    public RectTransform arrow;
    public RectTransform arrows;

    bool skip;
    
    public void SetTutorialText(string text, int pos = 0)
    {
        TutorialText.text = text;
        TutorialText.rectTransform.anchoredPosition = new Vector2(0, pos);
    }
    
    public void Show(RectTransform at = null)
    {
        gameObject.SetActive(true);
        //
        // var rect = at.rect;
        // mask.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rect.width);
        // mask.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, rect.height);
        // mask.anchoredPosition = at.anchoredPosition;
        
        
        // Set the mask size to match the target RectTransform's size
        var rect = at?.rect ?? new Rect();
        
        mask.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rect.width);
        mask.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, rect.height);

        // Convert the target RectTransform's world position to screen position
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(null, at?.position ?? Vector3.zero);

        // Convert the screen position to local position relative to the parent canvas
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)mask.parent, screenPoint, null, out localPoint);

        // Set the mask's anchored position to the calculated local point
        mask.anchoredPosition = localPoint;
        arrow.anchoredPosition = localPoint;
        arrow.gameObject.SetActive(at != null);
        
        G.ui.Punch(arrow);
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
        arrows.anchoredPosition = new Vector2(0, 161+Mathf.Abs(Mathf.Sin(Time.time) * 40f));
        
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
