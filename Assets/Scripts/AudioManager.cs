using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource _backGroundAudioSource;
    [SerializeField] private AudioSource _effectAudioSource;

    [SerializeField] private AudioClip _menuMusic;

    [SerializeField] private List<AudioClip> _gamePlayMusicList;
    private AudioClip _selectedGameplayClip;

    [SerializeField] private AudioClip _jumpClip;
    [SerializeField] private AudioClip _coinClip;
    [SerializeField] private AudioClip _winClip;
    [SerializeField] private AudioClip _hitClip;
    [SerializeField] private AudioClip _deathClip;

    [SerializeField] private Slider _bgSlider;
    [SerializeField] private Slider _fxSlider;
    [SerializeField] private Toggle _bgToggle;
    [SerializeField] private Toggle _fxToggle;

    private static AudioManager _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);

            if (_effectAudioSource == null)
            {
                _effectAudioSource = gameObject.AddComponent<AudioSource>();
            }
            if (_backGroundAudioSource == null)
            {
                _backGroundAudioSource = gameObject.AddComponent<AudioSource>();
            }
            GetValueAudio();
            OnValueChange();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        PlayMusicBasedOnScene(SceneManager.GetActiveScene().name);

        _bgSlider.value = PlayerPrefs.GetFloat("BackGroundVolume", 1f);
        _fxSlider.value = PlayerPrefs.GetFloat("EffectVolume", 1f);
        _bgToggle.isOn = PlayerPrefs.GetInt("BackGroundMuted", 0) == 1;
        _fxToggle.isOn = PlayerPrefs.GetInt("EffectMuted", 0) == 1;

        SetBackGroundVolume(_bgSlider.value);
        SetEffectAudioVolume(_fxSlider.value);

        
    }
    #region SettingPlayAudio
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
        PlayMusicBasedOnScene(scene.name);
        GetValueAudio();
    }
    private void PlayMusicBasedOnScene(string sceneName)
    {
        if (_backGroundAudioSource.isPlaying)
            _backGroundAudioSource.Stop();

        if (sceneName == "MainMenu")
        {
            _backGroundAudioSource.clip = _menuMusic;
            _backGroundAudioSource.loop = true;
        }
        else if (sceneName.StartsWith("GamePlay"))
        {
            if(_gamePlayMusicList != null && _gamePlayMusicList.Count > 0)
            {
                _selectedGameplayClip = _gamePlayMusicList[UnityEngine.Random.Range(0, _gamePlayMusicList.Count)];
                _backGroundAudioSource.clip = _selectedGameplayClip;
                _backGroundAudioSource.loop = true;
            }
        }
        _backGroundAudioSource.Play();
    }
    public void PlayCoinSound()
    {
        if (_coinClip == null || _effectAudioSource.mute) return;

        _effectAudioSource.PlayOneShot(_coinClip);
    }
    public void PlayJumpSound()
    {
        if (_jumpClip == null || _effectAudioSource.mute) return;

        _effectAudioSource?.PlayOneShot(_jumpClip);
    }
    public void PlayWinSound()
    {
        if (_winClip == null || _effectAudioSource.mute) return;
        _backGroundAudioSource.Stop();
        _effectAudioSource.PlayOneShot(_winClip);
    }
    public void PlayHitSound()
    {
        if (_hitClip == null || _effectAudioSource.mute) return;
        _effectAudioSource?.PlayOneShot(_hitClip);
    }
    public void PlayDeathSound()
    {
        if (_deathClip == null || _effectAudioSource.mute) return;
        _backGroundAudioSource?.Stop();
        _effectAudioSource?.PlayOneShot(_deathClip);
    }
    #endregion
    private void GetValueAudio()
    {
        _backGroundAudioSource.volume = PlayerPrefs.GetFloat("BackGroundVolume", 1f);
        _effectAudioSource.volume = PlayerPrefs.GetFloat("EffectVolume", 1f);
        _backGroundAudioSource.mute = PlayerPrefs.GetInt("BackgroundMuted", 0) == 1;
        _effectAudioSource.mute = PlayerPrefs.GetInt("EffectMuted", 0) == 1;
    }
    private void OnValueChange()
    {
        _bgSlider.onValueChanged.AddListener(SetBackGroundVolume);
        _fxSlider.onValueChanged.AddListener(SetEffectAudioVolume);

        _bgToggle.onValueChanged.AddListener(MuteBackGround);
        _fxToggle.onValueChanged.AddListener(MuteEffect);
    }
    #region SettingVolume
    public void SetBackGroundVolume(float volume)
    {
        _backGroundAudioSource.volume = volume;
        PlayerPrefs.SetFloat("BackGroundVolume", volume);
        PlayerPrefs.Save();
    }
    public void SetEffectAudioVolume(float volume)
    {
        _effectAudioSource.volume = volume;
        PlayerPrefs.SetFloat("EffectVolume", volume);
        PlayerPrefs.Save();
    }
    public void MuteBackGround(bool isMute)
    {
        _backGroundAudioSource.mute = isMute;
        PlayerPrefs.SetInt("BackGroundMuted", isMute ? 1 : 0);
        PlayerPrefs.Save();
    }
    public void MuteEffect(bool isMute)
    {
        _effectAudioSource.mute = isMute;
        PlayerPrefs.SetInt("EffectMuted", isMute ? 1 : 0);
        PlayerPrefs.Save();
    }
    #endregion
}
