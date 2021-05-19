using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameModeWindow : MonoBehaviour, IWindow
{
    [SerializeField] private Button _teamFightButton;
    [SerializeField] private Button _deathMatchButton;
    [SerializeField] private Button _closeButton;

    [SerializeField] private float _openCloseOffcet;
    [SerializeField] private float _openCloseTime;

    private bool _isOpened;

    public void CloseWindow()
    {
        if (_isOpened)
        {
            MyUtilities.UI.MoveRight(transform, _openCloseOffcet, _openCloseTime);
            _isOpened = false;
        }
    }

    public void OpenWindow()
    {
        if (!_isOpened)
        {
            MyUtilities.UI.MoveLeft(transform, _openCloseOffcet, _openCloseTime);
            _isOpened = true;
        }
    }

    private void Awake()
    {
        _teamFightButton.onClick.AddListener(OnTeamFigthClickHandler);
        _deathMatchButton.onClick.AddListener(OnDeathMatchClickHandler);
        _closeButton.onClick.AddListener(OnCloseButtonClickHandler);

    }

    private void Start()
    {
        EventAggregator.Post(this, new SetWindow { Window = this });
    }

    private void OnCloseButtonClickHandler()
    {
        CloseWindow();
        ServiceLocator.Resolve<UIManager>().MainWindow.OpenWindow();
    }

    private void OnDeathMatchClickHandler()
    {
        throw new NotImplementedException();
    }

    private void OnTeamFigthClickHandler()
    {
        throw new NotImplementedException();
    }
}
