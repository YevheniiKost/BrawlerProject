using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartGameCoundtown : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private Image _darkImage;
    private void Awake()
    {
        EventAggregator.Subscribe<OnStartGameScene>(StartCountdown);
    }

    private void OnDestroy()
    {
        EventAggregator.Unsubscribe<OnStartGameScene>(StartCountdown);
    }

    private void StartCountdown(object arg1, OnStartGameScene data)
    {
        StartCoroutine(CountDown(data.GameStartTime));
    }

    private IEnumerator CountDown(float gameStartTime)
    {
        while(gameStartTime > -1)
        {
            if (gameStartTime != 0)
                _text.text = gameStartTime.ToString();
            else
                _text.text = "To Battle!";
            gameStartTime--;
            ServiceLocator.Resolve<AudioManager>().PlaySFX(SoundsFx.StartBattle);

            yield return new WaitForSeconds(1);
        }

        EventAggregator.Post(this, new OnStartGame { });
        this.gameObject.SetActive(false);
    }
}
