using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsWindow : MonoBehaviour,IWindow
{
    [SerializeField] private Button _closeButton;
    [SerializeField] private Toggle _soundOnToggle;
    [SerializeField] private Toggle _musicOnToggle;
    [SerializeField] private Slider _soundsVolumeSlider;
    [SerializeField] private Slider _musicVolumeSlider;

    private AudioManager _audioManager;

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
        _closeButton.onClick.AddListener(OnCloseButtonClickHandler);
        _soundOnToggle.onValueChanged.AddListener(SoundToggleHandler);
        _musicOnToggle.onValueChanged.AddListener(MusicToggleHandler);
        _soundsVolumeSlider.onValueChanged.AddListener(SoudVolumeSliderHandler);
        _musicVolumeSlider.onValueChanged.AddListener(MusicVolumeSliderHandler);
    }

    private void Start()
    {
        _audioManager = ServiceLocator.Resolve<AudioManager>();

        _soundOnToggle.isOn = _audioManager.IsSoundsOn;
        _musicOnToggle.isOn = _audioManager.IsMusicOn;
        _soundsVolumeSlider.value = _audioManager.SoundFXVolume;
        _musicVolumeSlider.value = _audioManager.MusicVolume;

        EventAggregator.Post(this, new SetWindow { Window = this });
        CloseWindow();
    }

    private void OnDestroy()
    {
        EventAggregator.Post(this, new RemoveWindow { Window = this });
    }

    private void SoundToggleHandler(bool arg0) => _audioManager.SetSoundFX(arg0);

    private void MusicToggleHandler(bool arg0) => _audioManager.SetMusic(arg0);

    private void SoudVolumeSliderHandler(float arg0) => _audioManager.SetFXVolume(arg0);

    private void MusicVolumeSliderHandler(float arg0) => _audioManager.SetMusicVolume(arg0);

    private void OnCloseButtonClickHandler() => CloseWindow();
}
