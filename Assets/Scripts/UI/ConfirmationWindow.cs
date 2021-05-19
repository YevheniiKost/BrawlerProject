using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmationWindow : MonoBehaviour, IWindow
{
    [SerializeField] private TextMeshProUGUI _messageText;
    [SerializeField] private Button _yesButton;
    [SerializeField] private Button _noButton;

    private Action _onYesButtonCallback;
    public void OpenWindow()
    {
        gameObject.SetActive(true);
    }

    public void CloseWindow()
    {
        gameObject.SetActive(false);
    }

    public void CreateMessageWindow(string messageText, Action yesButtonCallback)
    {
        gameObject.SetActive(true);
        if (messageText != " ")
        {
            _messageText.text = messageText;
        }
        else
        {
            _messageText.text = $"Are you sure?";
        }

        _onYesButtonCallback = yesButtonCallback;
    }
    private void Awake()
    {
        _yesButton.onClick.AddListener(OnYesButtonClickHandler);
        _noButton.onClick.AddListener(OnNoButtonClickHandler);
    }

    private void Start()
    {
        EventAggregator.Post(this, new SetWindow { Confirmation = this });
        CloseWindow();
    }

    private void OnNoButtonClickHandler()
    {
        CloseWindow();
    }

    private void OnYesButtonClickHandler()
    {
        _onYesButtonCallback.Invoke();
    }
}
