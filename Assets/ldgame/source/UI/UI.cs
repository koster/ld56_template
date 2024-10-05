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
}