using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class DiceState
{
    public CMSEntity model;
    public int rollValue;
    public InteractiveObject view;
    public bool isPlayed;
}

public class InteractiveObject : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public SpriteRenderer spriteRenderer;
    public GameObject shadow;

    public DiceState state;

    public TMP_Text value;

    public MoveableBase moveable;
    public DraggableSmoothDamp draggable;

    public DiceZone zone;

    public int order;

    void Awake()
    {
        value.text = "";
        draggable = GetComponent<DraggableSmoothDamp>();
    }

    public void SetState(DiceState diceState)
    {
        state = diceState;
        state.view = this;

        if (state.model.Is<TagTint>(out var tint))
            spriteRenderer.color = tint.color;
    }

    public void SetValue(int val)
    {
        state.rollValue = val;
        value.text = val.ToString();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (zone != null)
        {
            zone.OnClickDice?.Invoke(this);
        }
    }

    void Update()
    {
        var sortingGroup = GetComponent<SortingGroup>();
        if (sortingGroup != null)
            sortingGroup.sortingOrder = order;
        shadow?.SetActive(zone?.isShadow ?? false);
    }

    public void Punch()
    {
        transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.2f);
    }

    public void Leave()
    {
        if (zone != null)
            zone.Release(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        var desc = TryGetSomethingDesc();
        if (!string.IsNullOrEmpty(desc))
            G.hud.tooltip.Show(desc);
    }

    string TryGetSomethingDesc()
    {
        if (state != null)
        {
            if (state.model.Is<TagDescription>(out var td))
                return td.loc;
        }

        return null;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        G.hud.tooltip.Hide();
    }
}