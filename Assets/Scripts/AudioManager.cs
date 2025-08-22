using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

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
    [SerializeField] private AudioClip _explosionClip;
    [SerializeField] private AudioClip _fireballClip;

    [Header("UI Controls")]
    [SerializeField] private Slider _bgSlider;
    [SerializeField] private Slider _fxSlider;
    [SerializeField] private Toggle _bgToggle;
    [SerializeField] private Toggle _fxToggle;

    [Header("Sprites")]
    [SerializeField] private Sprite _backGroundOnSprite;
    [SerializeField] private Sprite _backGroundOffSprite;

    [SerializeField] private Sprite _effectOnSprite;
    [SerializeField] private Sprite _effectOffSprite;

    private static AudioManager _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);

            if (_effectAudioSource == null)
                _effectAudioSource = gameObject.AddComponent<AudioSource>();
            if (_backGroundAudioSource == null)
                _backGroundAudioSource = gameObject.AddComponent<AudioSource>();

            GetValueAudio();
            OnValueChange();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        PlayMusicBasedOnScene(SceneManager.GetActiveScene().name);

        // Load Slider values
        _bgSlider.value = PlayerPrefs.GetFloat("BackGroundVolume", 1f);
        _fxSlider.value = PlayerPrefs.GetFloat("EffectVolume", 1f);

        // Load Toggle state (true = unmute, false = mute)
        bool bgMuted = PlayerPrefs.GetInt("BackGroundMuted", 0) == 1;
        bool fxMuted = PlayerPrefs.GetInt("EffectMuted", 0) == 1;

        _bgToggle.isOn = !bgMuted;
        _fxToggle.isOn = !fxMuted;

        SetBackGroundVolume(_bgSlider.value);
        SetEffectAudioVolume(_fxSlider.value);

        UpdateToggleIcon(_bgToggle, bgMuted, _backGroundOnSprite, _backGroundOffSprite);
        UpdateToggleIcon(_fxToggle, fxMuted, _effectOnSprite, _effectOffSprite);
    }

    #region Scene Audio
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
            if (_gamePlayMusicList != null && _gamePlayMusicList.Count > 0)
            {
                _selectedGameplayClip = _gamePlayMusicList[UnityEngine.Random.Range(0, _gamePlayMusicList.Count)];
                _backGroundAudioSource.clip = _selectedGameplayClip;
                _backGroundAudioSource.loop = true;
            }
        }
        _backGroundAudioSource.Play();
    }
    #endregion

    #region Play SFX
    public void PlayCoinSound() { if (_coinClip && !_effectAudioSource.mute) _effectAudioSource.PlayOneShot(_coinClip); }
    public void PlayJumpSound() { if (_jumpClip && !_effectAudioSource.mute) _effectAudioSource.PlayOneShot(_jumpClip); }
    public void PlayWinSound() { if (_winClip && !_effectAudioSource.mute) { _backGroundAudioSource.Stop(); _effectAudioSource.PlayOneShot(_winClip); } }
    public void PlayHitSound() { if (_hitClip && !_effectAudioSource.mute) _effectAudioSource.PlayOneShot(_hitClip); }
    public void PlayDeathSound() { if (_deathClip && !_effectAudioSource.mute) { _backGroundAudioSource.Stop(); _effectAudioSource.PlayOneShot(_deathClip); } }
    public void PlayExplosionSound() { if (_explosionClip && !_effectAudioSource.mute) _effectAudioSource.PlayOneShot(_explosionClip); }
    public void PlayFireBallSound() { if (_fireballClip && !_effectAudioSource.mute) _effectAudioSource.PlayOneShot(_fireballClip); }
    #endregion

    #region Settings
    private void GetValueAudio()
    {
        _backGroundAudioSource.volume = PlayerPrefs.GetFloat("BackGroundVolume", 1f);
        _effectAudioSource.volume = PlayerPrefs.GetFloat("EffectVolume", 1f);
        _backGroundAudioSource.mute = PlayerPrefs.GetInt("BackGroundMuted", 0) == 1;
        _effectAudioSource.mute = PlayerPrefs.GetInt("EffectMuted", 0) == 1;
    }

    private void OnValueChange()
    {
        _bgSlider.onValueChanged.AddListener(SetBackGroundVolume);
        _fxSlider.onValueChanged.AddListener(SetEffectAudioVolume);

        _bgToggle.onValueChanged.AddListener(OnBackGroundToggleChanged);
        _fxToggle.onValueChanged.AddListener(OnEffectToggleChanged);
    }

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

    private void OnBackGroundToggleChanged(bool isOn)
    {
        bool isMute = !isOn;
        _backGroundAudioSource.mute = isMute;
        PlayerPrefs.SetInt("BackGroundMuted", isMute ? 1 : 0);
        PlayerPrefs.Save();

        UpdateToggleIcon(_bgToggle, isMute, _backGroundOnSprite, _backGroundOffSprite);
    }

    private void OnEffectToggleChanged(bool isOn)
    {
        bool isMute = !isOn;
        _effectAudioSource.mute = isMute;
        PlayerPrefs.SetInt("EffectMuted", isMute ? 1 : 0);
        PlayerPrefs.Save();

        UpdateToggleIcon(_fxToggle, isMute, _effectOnSprite, _effectOffSprite);
    }

    private void UpdateToggleIcon(Toggle toggle, bool isMute, Sprite onSprite, Sprite offSprite)
    {
        Image image = toggle.targetGraphic as Image;
        if (image != null)
        {
            image.rectTransform.localEulerAngles = Vector3.zero;
            image.rectTransform.DORotate(new Vector3(0, 180, 0), 0.25f)
                .SetEase(Ease.InOutQuad)
                .OnComplete(() =>
                {
                    image.sprite = isMute ? offSprite : onSprite;
                    image.rectTransform.DORotate(Vector3.zero, 0.25f).SetEase(Ease.InOutQuad);
                });
        }
    }
    #endregion
}
