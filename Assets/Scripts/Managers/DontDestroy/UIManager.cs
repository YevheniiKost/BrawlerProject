using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;


    //Start scene windows
    public IWindow StartSceneWindow;
    public IWindow StartSceneSettinsWindow;
    public IWindow AboutAuthorWindow;

    //Main menu scene windows
    public IWindow MainWindow;
    public IWindow GameModWindow;
    public IWindow SelectHeroWindow;

    private ConfirmationWindow _confirmationWindow;

    public void CreateConfirmationWindow(Action action, string messageText)
    {
        _confirmationWindow.OpenWindow();
        _confirmationWindow.CreateMessageWindow(messageText, action);
    }

    private void Awake()
    {
        DontDestroyOnLoad(this);

        if (Instance != null)
            Destroy(this.gameObject);
        else
            Instance = this;

        ServiceLocator.Register(this);

        EventAggregator.Subscribe<SetWindow>(OnSetWindow);
        EventAggregator.Subscribe<OnGameModClick>(OnGameModClickHandler);
        EventAggregator.Subscribe<OnSelectHeroClick>(OnSelectHeroClickHandler);
    }

    private void OnSelectHeroClickHandler(object arg1, OnSelectHeroClick arg2)
    {
        MainWindow.CloseWindow();
        SelectHeroWindow.OpenWindow();
    }

    private void OnGameModClickHandler(object arg1, OnGameModClick arg2)
    {
        GameModWindow?.OpenWindow();
        MainWindow.CloseWindow();
    }

    private void OnSetWindow(object obj, SetWindow window)
    {
        if (obj.GetType() == typeof(MainWindow))
        {
            MainWindow = window.Window;
        }else if(obj.GetType() == typeof(GameModeWindow))
        {
            GameModWindow = window.Window;
        }else if(obj.GetType() == typeof(SelectHeroWindow))
        {
            SelectHeroWindow = window.Window;
        }else if(obj.GetType() == typeof(StartSceneWindow))
        {
            StartSceneWindow = window.Window;
        } else if(obj.GetType() == typeof(StartSceneSettingsWindow))
        {
            StartSceneSettinsWindow = window.Window;
        } else if(obj.GetType() == typeof(AboutAuthorWindow))
        {
            AboutAuthorWindow = window.Window;
        }else if(obj.GetType () == typeof(ConfirmationWindow))
        {
            _confirmationWindow = window.Confirmation;
        }
        else
        {
            return;
        }

    }

}

public interface IWindow
{
    void OpenWindow();
    void CloseWindow();
}
