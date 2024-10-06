using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class XKLogoAnimation : MonoBehaviour
{
    public TMP_Text text;
    public SpriteRenderer logo;

    IEnumerator Start()
    {
        if (GameSettings.SKIP_INTRO)
        {
            SceneManager.LoadScene(GameSettings.MAIN_SCENE);
        }
        
        text.alpha = 0f;

        text.text = "GAME BY XK";
        
        yield return new WaitForSeconds(0.5f);

        G.feel.UIPunchSoft();
        
        text.DOFade(1f, 0.5f);

        yield return new WaitForSeconds(1.5f);
        
        G.feel.UIPunchSoft();

        text.DOFade(0f, 1f);
        
        yield return new WaitForSeconds(1f);
        
        text.text = "<size=50%>MADE IN 48 HOURS FOR\nLUDUM DARE 56</size>";
        
        G.feel.UIPunchSoft();
        
        text.DOFade(1f, 0.5f);
        
        yield return new WaitForSeconds(3.5f);
        
        G.feel.UIPunchSoft();
        
        text.DOFade(0f, 1.5f);
        
        yield return new WaitForSeconds(1.5f);
        
        G.feel.UIPunchSoft();

        logo.DOFade(1f, 1f);

        yield return new WaitForSeconds(3f);

        logo.DOFade(0f, 2f);

        yield return new WaitForSeconds(2f);
        
        SceneManager.LoadScene(GameSettings.MAIN_SCENE);
    }
}
