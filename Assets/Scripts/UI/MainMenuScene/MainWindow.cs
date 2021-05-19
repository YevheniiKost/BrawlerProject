using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainWindow : MonoBehaviour, IWindow
{
    [SerializeField] private Button _gameModeButton;
    [SerializeField] private Button _selectHeroButton;
    [SerializeField] private Button _playButtonHeroButton;
    [SerializeField] private Button _backToMainMenuButton;

    [SerializeField] private Transform _gameModePanel;
    [SerializeField] private Transform _heroPanel;
    [SerializeField] private Transform _backPanel;
    [SerializeField] private Transform _playPanel;

    [SerializeField] private float _closeWindowPanelsOffcet = 200f;
    [SerializeField] private float _openCloseTime = 2f;

    private bool _isOpened;

    public void CloseWindow()
    {
        if (_isOpened)
        {
            MyUtilities.UI.MoveRight(_backPanel, _closeWindowPanelsOffcet, _openCloseTime);
            MyUtilities.UI.MoveRight(_playPanel, _closeWindowPanelsOffcet, _openCloseTime);
            MyUtilities.UI.MoveLeft(_gameModePanel, _closeWindowPanelsOffcet, _openCloseTime);
            MyUtilities.UI.MoveLeft(_heroPanel, _closeWindowPanelsOffcet, _openCloseTime);
            _isOpened = false;
        }
    }

    public void OpenWindow()
    {
        if (!_isOpened)
        {
            MyUtilities.UI.MoveLeft(_backPanel.transform, _closeWindowPanelsOffcet, _openCloseTime);
            MyUtilities.UI.MoveLeft(_playPanel.transform, _closeWindowPanelsOffcet, _openCloseTime);
            MyUtilities.UI.MoveRight(_gameModePanel.transform, _closeWindowPanelsOffcet, _openCloseTime);
            MyUtilities.UI.MoveRight(_heroPanel.transform, _closeWindowPanelsOffcet, _openCloseTime);
            _isOpened = true;
        }
    }

    private void Awake()
    {
        _gameModeButton.onClick.AddListener(OnGameModeButtonClickHandler);
        _selectHeroButton.onClick.AddListener(OnSelectHeroButtonClickHandler);
        _playButtonHeroButton.onClick.AddListener(OnPlayButtonClickHandler);
        _backToMainMenuButton.onClick.AddListener(OnBackButtonClickHandler);

        _isOpened = true;
    }

    private void Start()
    {
        EventAggregator.Post(this, new SetWindow { Window = this });
    }


    private void OnGameModeButtonClickHandler()
    {
        EventAggregator.Post(this, new OnGameModClick { });
    }

    private void OnSelectHeroButtonClickHandler()
    {
        EventAggregator.Post(this, new OnSelectHeroClick { });
    }

    private void OnPlayButtonClickHandler()
    {
        EventAggregator.Post(this, new OnPlayClick { });
    }

    private void OnBackButtonClickHandler()
    {
        EventAggregator.Post(this, new OnReturnToMainMenuClick { });
    }
}
