using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartSceneSettingsWindow : MonoBehaviour,IWindow
{
    [SerializeField] private Button _closeButton;
    [SerializeField] private Toggle _soundOnToggle;
    [SerializeField] private Toggle _musicOnToggle;
    [SerializeField] private Slider _soundsVolumeSlider;
    [SerializeField] private Slider _musicVolumeSlider;


    private void Awake()
    {
        _closeButton.onClick.AddListener(OnCloseButtonClickHandler);
        _soundOnToggle.onValueChanged.AddListener(SoundToggleHandler);
        _musicOnToggle.onValueChanged.AddListener(MusicToggleHandler);
        _soundsVolumeSlider.onValueChanged.AddListener(SoudVolumeSliderHandler);
        _musicVolumeSlider.onValueChanged.AddListener(MusicVolumeSliderHandler);
    }

    private void SoundToggleHandler(bool arg0)
    {
        throw new NotImplementedException();
    }

    private void MusicToggleHandler(bool arg0)
    {
        throw new NotImplementedException();
    }

    private void SoudVolumeSliderHandler(float arg0)
    {
        throw new NotImplementedException();
    }

    private void MusicVolumeSliderHandler(float arg0)
    {
        throw new NotImplementedException();
    }

    private void OnCloseButtonClickHandler()
    {
        CloseWindow();
    }

    public void CloseWindow()
    {
        gameObject.SetActive(false);
    }

    public void OpenWindow()
    {
        gameObject.SetActive(true);
    }

    private void Start()
    {
        EventAggregator.Post(this, new SetWindow { Window = this });
        CloseWindow();
    }
}
