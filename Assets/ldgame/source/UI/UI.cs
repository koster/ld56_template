using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    public TextMeshProUGUI debug_text;
    public UIPauseMenu pause;
    public GameObject win;
    public GameObject defeat;

    void Awake()
    {
        G.ui = this;
    }

    void Start()
    {
        Reset();
    }

    void Reset()
    {
        pause.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pause.Toggle();
        }
    }

    public void Punch(Transform healthTransform)
    {
        healthTransform.transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.2f);
    }

    public void QPunch(Transform healthTransform)
    {
        healthTransform.transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.1f);
    }

    public IEnumerator ScaleCountIn(Transform healthValueTransform)
    {
        healthValueTransform.transform.DOKill(true);
        yield return healthValueTransform.transform.DOScale(Vector3.one * 1.2f, 0.01f).WaitForCompletion();
    }
    
    public IEnumerator ScaleCountOut(Transform healthValueTransform)
    {
        healthValueTransform.transform.DOKill(true);
        yield return healthValueTransform.transform.DOScale(Vector3.one * 1f, 0.1f).WaitForCompletion();
    }
}