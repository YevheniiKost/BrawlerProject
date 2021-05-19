using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AboutAuthorWindow : MonoBehaviour, IWindow
{
    [SerializeField] private Button _closeButton;


    private void Awake()
    {
        _closeButton.onClick.AddListener(OnCloseButtonClickHandler);
    }

    private void OnCloseButtonClickHandler()
    {
        CloseWindow();
    }

    public void CloseWindow()
    {
        gameObject.SetActive(false);
    }

    public void OpenWindow()
    {
        gameObject.SetActive(true);
    }

    private void Start()
    {
        EventAggregator.Post(this, new SetWindow { Window = this });
        CloseWindow();
    }
}
