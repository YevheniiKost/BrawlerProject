using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public Dictionary<Type, IWindow> Windows = new Dictionary<Type, IWindow>();
    public ConfirmationWindow _confirmationWindow;

    public void CreateConfirmationWindow(Action action, string messageText)
    {
        OpenWindow(typeof(ConfirmationWindow));
        _confirmationWindow.CreateMessageWindow(messageText, action);
    }

    public void OpenWindow(Type windowType)
    {
        if (Windows.ContainsKey(windowType))
        {
            Windows[windowType].OpenWindow();
        } else
            Debug.LogError($"UI manager don`t have window of type {typeof(Type)}");
    }
    public void CloseWindow(Type windowType)
    {
        if (Windows.ContainsKey(windowType))
        {
            Windows[windowType].CloseWindow();
        }else
            Debug.LogError($"UI manager don`t have window of type {typeof(Type)}");
    }

    private void Awake()
    {
        DontDestroyOnLoad(this);

        if (Instance != null)
            Destroy(this.gameObject);
        else
            Instance = this;

        ServiceLocator.Register(this);

        SubscribeToEvents();
    }

    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }

    #region Click handlers

    private void PauseGameHandler(object arg1, OnGamePaused arg2)
    {
        OpenWindow(typeof(PauseWindow));
    }

    private void OnSelectHeroClickHandler(object arg1, OnSelectHeroClick arg2)
    {
        CloseWindow(typeof(MainWindow));
        OpenWindow(typeof(SelectHeroWindow));
    }

    private void OnGameModClickHandler(object arg1, OnGameModClick arg2)
    {
        OpenWindow(typeof(GameModeWindow));
        CloseWindow(typeof(MainWindow));
    }

    private void EndGameHandler(object arg1, OnEndGame arg2)
    {
        OpenWindow(typeof(EndGameWindow));
    }
    #endregion

    #region Set and remove windows
    private void OnSetWindow(object obj, SetWindow data)
    {
        if (!Windows.ContainsKey(obj.GetType()))
        {
            Windows.Add(obj.GetType(), data.Window);
        }
        else
        {
            Debug.LogError($"UI manager already contains window of type {obj.GetType()}");
        }
    }

    private void OnRemoveWindow(object obj, RemoveWindow data)
    {
        if (Windows.ContainsKey(obj.GetType()))
        {
            Windows.Remove(obj.GetType());
        }
        else
        {
            Debug.LogError($"UI manager don`t have window of type {obj.GetType()}");
        }
    }

    private void OnSetConfirmationWindow(object arg1, SetConfirmationWindow data)
    {
        _confirmationWindow = data.Window;
    }

    private void OnRemoveConfirmationWindow(object arg1, RemoveConfirmationWindow data)
    {
        _confirmationWindow = null;
    }

    #endregion

    private void SubscribeToEvents()
    {
        EventAggregator.Subscribe<SetWindow>(OnSetWindow);
        EventAggregator.Subscribe<OnGameModClick>(OnGameModClickHandler);
        EventAggregator.Subscribe<OnSelectHeroClick>(OnSelectHeroClickHandler);
        EventAggregator.Subscribe<RemoveWindow>(OnRemoveWindow);
        EventAggregator.Subscribe<SetConfirmationWindow>(OnSetConfirmationWindow);
        EventAggregator.Subscribe<RemoveConfirmationWindow>(OnRemoveConfirmationWindow);
        EventAggregator.Subscribe<OnGamePaused>(PauseGameHandler);
        EventAggregator.Subscribe<OnEndGame>(EndGameHandler);
    }

 

    private void UnsubscribeFromEvents()
    {
        EventAggregator.Unsubscribe<SetWindow>(OnSetWindow);
        EventAggregator.Unsubscribe<OnGameModClick>(OnGameModClickHandler);
        EventAggregator.Unsubscribe<OnSelectHeroClick>(OnSelectHeroClickHandler);
        EventAggregator.Unsubscribe<RemoveWindow>(OnRemoveWindow);
        EventAggregator.Unsubscribe<SetConfirmationWindow>(OnSetConfirmationWindow);
        EventAggregator.Unsubscribe<RemoveConfirmationWindow>(OnRemoveConfirmationWindow);
        EventAggregator.Unsubscribe<OnGamePaused>(PauseGameHandler);
        EventAggregator.Unsubscribe<OnEndGame>(EndGameHandler);
    }

   
}

public interface IWindow
{
    void OpenWindow();
    void CloseWindow();
    
}

public interface IConfirmationWindow : IWindow
{
    void CreateConfirmation();
}
