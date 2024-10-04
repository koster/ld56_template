using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class XKLogoAnimation : MonoBehaviour
{
    public TMP_Text text;

    IEnumerator Start()
    {
        if (GameSettings.SKIP_INTRO)
        {
            SceneManager.LoadScene(GameSettings.MAIN_SCENE);
        }
        
        text.alpha = 0f;

        text.text = "XK";
        
        yield return new WaitForSeconds(0.5f);
        
        G.audio.Play<SFX_Click>();
        text.DOFade(1f, 0.5f);

        yield return new WaitForSeconds(0.5f);
        
        G.audio.Play<SFX_Click>();
        text.DOFade(0f, 0.5f);
        
        yield return new WaitForSeconds(0.5f);
        
        text.text = "<size=50%>MADE IN 48 HOURS FOR\nLUDUM DARE 56</size>";
        
        G.audio.Play<SFX_Click>();
        text.DOFade(1f, 0.5f);
        
        yield return new WaitForSeconds(3.5f);
        
        G.audio.Play<SFX_Click>();
        text.DOFade(0f, 1.5f);
        
        yield return new WaitForSeconds(1.5f);
        
        G.audio.Play<SFX_Click>();
        
        SceneManager.LoadScene(GameSettings.MAIN_SCENE);
    }
}
