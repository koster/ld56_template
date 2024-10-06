using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
    public float fadeDuration = 1f; // Duration of fade in seconds
    private Canvas canvas;
    private Image fadeImage;
    
    private void Awake()
    {
        // Create a new Canvas and configure it
        canvas = new GameObject("ScreenFaderCanvas").AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 9;

        // Create the black Image to cover the screen
        fadeImage = new GameObject("FadeImage").AddComponent<Image>();
        fadeImage.transform.SetParent(canvas.transform, false);
        fadeImage.rectTransform.anchoredPosition = Vector2.zero;
        fadeImage.rectTransform.sizeDelta = new Vector2(5000, 3000);

        // Set the Image color to black and make it transparent initially
        fadeImage.color = new Color(0f, 0f, 0f, 1f);
        
        DontDestroyOnLoad(canvas.gameObject);
        
        FadeOut();
    }

    public void FadeIn()
    {
        StartCoroutine(Fade(0f, 1f)); // Fade from black (opaque) to transparent
    }

    public void FadeOut()
    {
        StartCoroutine(Fade(1f, 0f)); // Fade from transparent to black (opaque)
    }

    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float elapsed = 0f;

        Color fadeColor = fadeImage.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            fadeColor.a = Mathf.Lerp(startAlpha, endAlpha, elapsed / fadeDuration);
            fadeImage.color = fadeColor;
            yield return null;
        }

        // Ensure the final color is correctly set
        fadeColor.a = endAlpha;
        fadeImage.color = fadeColor;
    }
}