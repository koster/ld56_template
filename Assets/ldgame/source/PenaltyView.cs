using DG.Tweening;
using TMPro;
using UnityEngine;

public class PenaltyView : MonoBehaviour
{
    public ChallengeContainer container;
    public TMP_Text text;

    Transform target;
    Vector3 offset;
    Vector3 ls;

    void Start()
    {
        ls = text.transform.localScale;

        // target = transform.parent;
        // transform.parent = null;
        // offset = target.position - transform.position;
        transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.25f).SetDelay(0.5f);
    }

    public void Update()
    {
        // if (target == null)
        // {
        //     Destroy(gameObject);
        //     return;
        // }

        // transform.position = target.position - offset;

        if (container.model.Is<TagChallengePenalty>(out var tp))
        {
            text.transform.localScale = (Mathf.Sin(Time.time * 0.5f) * 0.1f + 1f) * ls;
            text.text = "-" + tp.damage + " ENERGY";
        }
        else
        {
            text.text = "";
        }
    }
}