using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class EndGameWindow : MonoBehaviour, IWindow
{
    [SerializeField] private Button _restartBattleButton;
    [SerializeField] private Button _returnToMainMenuButton;
    [SerializeField] private TextMeshProUGUI _congradText;

    public void CloseWindow()
    {
        gameObject.SetActive(false);
    }

    public void OpenWindow()
    {
        gameObject.SetActive(true);
    }

    private void Awake()
    {
        _restartBattleButton.onClick.AddListener(OnRestartButtonClickHandler);
        _returnToMainMenuButton.onClick.AddListener(OnReturnButtonClickHandler);

        EventAggregator.Subscribe<OnEndGame>(EndGameText);
    }

    private void OnDestroy()
    {
        EventAggregator.Unsubscribe<OnEndGame>(EndGameText);
        EventAggregator.Post(this, new RemoveWindow { });
    }
    private void Start()
    {
        EventAggregator.Post(this, new SetWindow { Window = this });
        CloseWindow();
    }
    private void EndGameText(object arg1, OnEndGame data)
    {
        if(data.Winner == Team.Red)
        {
            _congradText.text = "Defeat";
        } else
        {
            _congradText.text = "Victory";
        }
    }

    private void OnRestartButtonClickHandler()
    {
        EventAggregator.Post(this, new OnRestartTeamFirght { });
    }

    private void OnReturnButtonClickHandler()
    {
        EventAggregator.Post(this, new OnReturnToMainMenuClick { });
    }
}
