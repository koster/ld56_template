using TMPro;
using UnityEngine;

public class BuildInfoDisplay : MonoBehaviour
{
    void Start()
    {
        // Создаем объект TextMeshPro
        GameObject buildInfoTextObject = new GameObject("BuildInfoText");
        buildInfoTextObject.transform.SetParent(this.transform);

        // Добавляем TextMeshPro компонент
        TextMeshProUGUI buildInfoText = buildInfoTextObject.AddComponent<TextMeshProUGUI>();
        
        // Устанавливаем текст с информацией о билде
        buildInfoText.text = $"Build: {BuildInfo.BuildNumber}\nTimestamp: {BuildInfo.BuildTimestamp}";

        // Настраиваем позицию и параметры текста
        RectTransform rectTransform = buildInfoText.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(1, 1); // Верхний правый угол
        rectTransform.anchorMax = new Vector2(1, 1);
        rectTransform.pivot = new Vector2(1, 1); 
        rectTransform.anchoredPosition = new Vector2(-10, -10); // Немного отступаем от края

        // Настраиваем визуальные параметры текста
        buildInfoText.fontSize = 12;
        buildInfoText.alignment = TextAlignmentOptions.TopRight;
        buildInfoText.alpha = 0.02f;
    }
}