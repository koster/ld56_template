using UnityEngine;

public class CameraMeander : MonoBehaviour
{
    void Update()
    {
        if (G.ui.tutorial.gameObject.activeSelf)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, 0.1f);
        }
        else
        {
            var mousePositionX = Mathf.Clamp01(Input.mousePosition.x / Screen.width) - 0.5f;
            var mousePositionY = Mathf.Clamp01(Input.mousePosition.y / Screen.height) - 0.5f;
            transform.localPosition = new Vector3(mousePositionX, mousePositionY * 0.1f);
        }
    }
}
