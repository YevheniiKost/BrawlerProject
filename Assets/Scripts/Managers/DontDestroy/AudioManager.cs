using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource _soundFxSource;
    [SerializeField] private AudioSource _musicSource;

    [SerializeField] private List<SoundFxData> _soundsFXList = new List<SoundFxData>();
    [SerializeField] private List<MusicData> _musicList = new List<MusicData>();

    public static AudioManager Instance;

    public float MusicVolume => _musicVolume;
    public float SoundFXVolume => _soundsFXVolume;
    public bool IsMusicOn => _isMusicOn;
    public bool IsSoundsOn => _isSoundsOn;

    private float _musicVolume = 1;
    private float _soundsFXVolume = 1;

    private bool _isMusicOn;
    private bool _isSoundsOn;

    private const string _musicVolumeKey = "MusicVolume";
    private const string _soundVolumeKey = "SoundVolume";
    private const string _musicMuteKey = "MusicMute";
    private const string _sounsMuteKey = "SoundMute";

    public void PlaySFX(SoundsFx soundsFx)
    {
        var clip = GetSoundFXClip(soundsFx);
        _soundFxSource.PlayOneShot(clip);
    }

    public void PlayMusic(MusicType music)
    {
        _musicSource.clip = GetMusicClip(music);
        _musicSource.Play();
    }

    public void SetSoundFX(bool isOn)
    {
        _isSoundsOn = isOn;
        PlayerPrefs.SetInt(_sounsMuteKey, isOn ? 1 : 0);
        _soundFxSource.mute = !_isSoundsOn;
    }

    public void SetMusic(bool isOn)
    {
        _isMusicOn = isOn;
        PlayerPrefs.SetInt(_musicMuteKey, isOn ? 1 : 0);
        _musicSource.mute = !_isMusicOn;
    }

    public void SetMusicVolume(float value)
    {
        _musicVolume = value;
        PlayerPrefs.SetFloat(_musicVolumeKey, value);
        _musicSource.volume = _musicVolume;
    }

    public void SetFXVolume(float value)
    {
        _soundsFXVolume = value;
        PlayerPrefs.SetFloat(_soundVolumeKey, value);
        _soundFxSource.volume = _soundsFXVolume;
    }

    private void Awake()
    {
        DontDestroyOnLoad(this);

        if (Instance != null)
            Destroy(this.gameObject);
        else
            Instance = this;

        ServiceLocator.Register(this);
    }

    private void OnDestroy()
    {
        ServiceLocator.Unregister(this);
    }

    private void Start()
    {
        _musicVolume = PlayerPrefs.GetFloat(_musicVolumeKey);
        _soundsFXVolume = PlayerPrefs.GetFloat(_soundVolumeKey);
        _isMusicOn = PlayerPrefs.GetInt(_musicMuteKey) != 0;
        _isSoundsOn = PlayerPrefs.GetInt(_sounsMuteKey) != 0;

        _musicSource.volume = _musicVolume;
        _soundFxSource.volume = _musicVolume;
        _musicSource.mute = !_isMusicOn;
        _soundFxSource.mute = !_isSoundsOn;
    }
   
    private AudioClip GetSoundFXClip(SoundsFx soundsFx)
    {
        var soundData = _soundsFXList.Find(sfxData => sfxData.SoundFx == soundsFx);
        return soundData?.Clip;
    }
    private AudioClip GetMusicClip(MusicType music)
    {
        var musicData = _musicList.Find(musicfx => musicfx.Music == music);
        return musicData?.Clip;
    }

}

public enum SoundsFx
{
   
}

public enum MusicType
{
    MainMenu,
    Game
}

[Serializable]
public class SoundFxData
{
    public SoundsFx SoundFx;
    public AudioClip Clip;
}


[Serializable]
public class MusicData
{
    public MusicType Music;
    public AudioClip Clip;
}
