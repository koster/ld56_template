using UnityEngine;
using UnityEngine.UI;

public class Cursor : MonoBehaviour
{
    public Sprite up;
    public Sprite dn;
    public Image img;
    
    void Update()
    {
        img.sprite = Input.GetMouseButton(0) ? dn : up;
        GetComponent<RectTransform>().anchoredPosition = Input.mousePosition;
    }
}
