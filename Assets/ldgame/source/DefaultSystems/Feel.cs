using UnityEngine;

public class Feel : MonoBehaviour
{
    public void UIPunchSoft()
    {
        G.audio.Play<SFX_Click>();
        G.camera.UIHit();
    }
}