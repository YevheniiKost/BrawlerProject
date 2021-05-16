using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MainWindow : MonoBehaviour, IWindow
{
    [SerializeField] private Button _gameModeButton;
    [SerializeField] private Button _selectHeroButton;
    [SerializeField] private Button _playButtonHeroButton;
    [SerializeField] private Button _backToMainMenuButton;

    [SerializeField] private Image _gameModePanel;
    [SerializeField] private Image _heroPanel;
    [SerializeField] private Image _backPanel;
    [SerializeField] private Image _playPanel;

    [SerializeField] private float _closeWindowPanelsOffcet = 200f;
    [SerializeField] private float _openCloseTime = 2f;

    private bool _isOpened;

    public void CloseWindow()
    {
        if (_isOpened)
        {
            _backPanel.transform.DOBlendableLocalMoveBy(new Vector3(_closeWindowPanelsOffcet, 0, 0), _openCloseTime).SetEase(Ease.InCubic);
            _playPanel.transform.DOBlendableLocalMoveBy(new Vector3(_closeWindowPanelsOffcet, 0, 0), _openCloseTime).SetEase(Ease.InCubic);
            _gameModePanel.transform.DOBlendableLocalMoveBy(new Vector3(-_closeWindowPanelsOffcet, 0, 0), _openCloseTime).SetEase(Ease.InCubic);
            _heroPanel.transform.DOBlendableLocalMoveBy(new Vector3(-_closeWindowPanelsOffcet, 0, 0), _openCloseTime).SetEase(Ease.InCubic);
            _isOpened = false;
        }
    }

    public void OpenWindow()
    {
        if (!_isOpened)
        {
            _backPanel.transform.DOBlendableLocalMoveBy(new Vector3(-_closeWindowPanelsOffcet, 0, 0), _openCloseTime).SetEase(Ease.InCubic);
            _playPanel.transform.DOBlendableLocalMoveBy(new Vector3(-_closeWindowPanelsOffcet, 0, 0), _openCloseTime).SetEase(Ease.InCubic);
            _gameModePanel.transform.DOBlendableLocalMoveBy(new Vector3(_closeWindowPanelsOffcet, 0, 0), _openCloseTime).SetEase(Ease.InCubic);
            _heroPanel.transform.DOBlendableLocalMoveBy(new Vector3(_closeWindowPanelsOffcet, 0, 0), _openCloseTime).SetEase(Ease.InCubic);
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
