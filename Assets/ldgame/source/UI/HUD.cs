using System.Collections;
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

        StartCoroutine(TrackHealth());
    }

    IEnumerator TrackHealth()
    {
        Health.value = G.run.maxHealth / 2;
        while (true)
        {
            if (Health.value > G.run.health)
            {
                Health.value--;
                yield return UpdateHP();
            }

            if (Health.value < G.run.health)
            {
                Health.value++;
                yield return UpdateHP();
            }
            
            Health.maxValue = G.run.maxHealth;
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator UpdateHP()
    {
        yield return G.ui.ScaleCountIn(HealthValue.transform);
        HealthValue.text = Health.value + "/" + G.run.maxHealth;
        yield return G.ui.ScaleCountOut(HealthValue.transform);
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

    public void PunchEndTurn()
    {
        G.ui.Punch(EndTurn.transform);
    }
}