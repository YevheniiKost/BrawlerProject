using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseWindow : MonoBehaviour, IWindow
{
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _returnButton;
    [SerializeField] private Button _closeButton;

    public void OpenWindow()
    {
        gameObject.SetActive(true);
    }

    public void CloseWindow()
    {
        gameObject.SetActive(false);
    }

    private void Awake()
    {
        _restartButton.onClick.AddListener(OnRestartButtonClickHandler);
        _settingsButton.onClick.AddListener(OnSettingsClickHandler);
        _returnButton.onClick.AddListener(OnReturnButtonClickHandler);
        _closeButton.onClick.AddListener(OnClosButtonClickHandler);
    }

    private void Start()
    {
        EventAggregator.Post(this, new SetWindow { Window = this });
        CloseWindow();
    }

    private void OnDestroy()
    {
        EventAggregator.Post(this, new RemoveWindow { Window = this });
    }

   
    private void OnRestartButtonClickHandler()
    {
        EventAggregator.Post(this, new OnRestartTeamFirght { });
    }

    private void OnSettingsClickHandler()
    {
        ServiceLocator.Resolve<UIManager>().OpenWindow(typeof(SettingsWindow));
    }

    private void OnReturnButtonClickHandler()
    {
        EventAggregator.Post(this, new OnReturnToMainMenuClick { });
    }

    private void OnClosButtonClickHandler()
    {
        EventAggregator.Post(this, new OnGameUnpased { });
        CloseWindow();
    }


}
