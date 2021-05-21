using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartSceneWindow : MonoBehaviour, IWindow
{
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _aboutAuthorButton;
    [SerializeField] private Button _quitButton;

    private UIManager _uiManager;

    #region Open close
    public void OpenWindow()
    {
        throw new NotImplementedException();
    }

    public void CloseWindow()
    {
        throw new NotImplementedException();
    }
    #endregion

    private void Awake()
    {
        _playButton.onClick.AddListener(OnPlayButtonClickHandler);
        _settingsButton.onClick.AddListener(OnSettingsButtonClickHandler);
        _aboutAuthorButton.onClick.AddListener(OnAuthorButtonClickHandler);
        _quitButton.onClick.AddListener(OnExitButtonClickHandler);
    }

    private void Start()
    {
        EventAggregator.Post(this, new SetWindow { Window = this });
        _uiManager = ServiceLocator.Resolve<UIManager>();
    }

    private void OnDestroy()
    {
        EventAggregator.Post(this, new RemoveWindow { Window = this });
    }

    private void OnPlayButtonClickHandler()
    {
        EventAggregator.Post(this, new OnStartScenePlayClick { });
    }

    private void OnSettingsButtonClickHandler()
    {
        _uiManager.OpenWindow(typeof(SettingsWindow));
    }

    private void OnAuthorButtonClickHandler()
    {
        _uiManager.OpenWindow(typeof(AboutAuthorWindow));
    }

    private void OnExitButtonClickHandler()
    {
        EventAggregator.Post(this, new OnExitGameClickes { });
    }

   
}
