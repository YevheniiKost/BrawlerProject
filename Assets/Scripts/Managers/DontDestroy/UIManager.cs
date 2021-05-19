using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public IWindow MainWindow;
    public IWindow GameModWindow;
    public IWindow SelectHeroWindow;

    private void Awake()
    {
        DontDestroyOnLoad(this);

        if (Instance != null)
            Destroy(this);
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
