using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UpperPanelInGame : MonoBehaviour, IGameTimer 
{
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private TextMeshProUGUI _redTeamPoints;
    [SerializeField] private TextMeshProUGUI _blueTeamPoints;
    [SerializeField] private Button _pauseButton;

    public void UpdateTimer(float currentTime)
    {
        int min = Mathf.FloorToInt(currentTime / 60);
        int sec = Mathf.FloorToInt(currentTime % 60);
        _timerText.text = min.ToString("00") + ":" + sec.ToString("00");
    }

    private void Awake()
    {
        EventAggregator.Subscribe<UpdateCrystalCounter>(UpdatePointsHandler);
        _pauseButton.onClick.AddListener(OnPauseButtonClickHandler);
    }

    private void OnPauseButtonClickHandler()
    {
        EventAggregator.Post(this, new OnGamePaused { });
    }

    private void OnDestroy()
    {
        EventAggregator.Unsubscribe<UpdateCrystalCounter>(UpdatePointsHandler);
    }

    private void UpdatePointsHandler(object arg1, UpdateCrystalCounter data)
    {
        if(data.CurrentTeam == Team.Red)
        {
            _redTeamPoints.text = data.CurrentCrystalCount.ToString();
        } else if(data.CurrentTeam == Team.Blue)
        {
            _blueTeamPoints.text = data.CurrentCrystalCount.ToString();
        }
    }
}

public interface IGameTimer
{
    void UpdateTimer(float currentTime);
}
