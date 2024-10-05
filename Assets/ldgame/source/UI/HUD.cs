using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public TextMeshProUGUI DiceView;
    public Button EndTurn;
    public UITooltip tooltip;

    public Slider Health;
    public TMP_Text HealthValue;

    void Awake()
    {
        G.hud = this;
        G.hud.tooltip.Hide();
    }

    void Start()
    {
        EndTurn.onClick.AddListener(OnClickEndTurn);
    }

    void OnClickEndTurn()
    {
        G.main.EndTurn();
    }

    public void DisableHud()
    {
        EndTurn.interactable = false;
    }

    public void EnableHud()
    {
        EndTurn.interactable = true;
    }

    void Update()
    {
        DiceView.text = "Dice left:" + G.main.diceBag.Count;
        Health.value = G.run.health;
        Health.maxValue = G.run.maxHealth;
        HealthValue.text = G.run.health + "/" + G.run.maxHealth;
    }

    public static Vector2 MousePositionToCanvasPosition(Canvas canvas, RectTransform rectTransform)
    {
        Vector2 localPoint;
        Vector2 screenPosition = Input.mousePosition;
        Camera uiCamera = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform,
            screenPosition,
            uiCamera,
            out localPoint
        );

        return localPoint;
    }

    public Vector2 MousePos()
    {
        return MousePositionToCanvasPosition(G.hud.GetComponent<Canvas>(), G.hud.GetComponent<RectTransform>());
    }
}