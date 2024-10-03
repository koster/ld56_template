using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPauseMenu : MonoBehaviour
{
    public void Toggle()
    {
        if (gameObject.activeSelf)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }

    void Show()
    {
        G.IsPaused = true;
        gameObject.SetActive(true);
    }

    void Hide()
    {
        G.IsPaused = true;
        gameObject.SetActive(false);
    }
}