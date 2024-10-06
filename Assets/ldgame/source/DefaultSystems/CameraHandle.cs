using System;
using DG.Tweening;
using UnityEngine;

public class CameraHandle : MonoBehaviour
{
    public void Shake(float i, float t)
    {
        Camera.main.DOShakePosition(t, i, 10, 45f);
    }

    public void UIHit()
    {
        Shake(0.025f, 0.4f);
    }
}