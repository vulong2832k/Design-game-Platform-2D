using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [Header("Text GamePlay: ")]
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _mapNameText;
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private string _nameMapText = "???";

    [Header("Text Victory Panel: ")]
    [SerializeField] private TextMeshProUGUI _victoryCoinText;
    [SerializeField] private TextMeshProUGUI _victoryTimeText;

    [Header("Panel: ")]
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private GameObject _victoryPanel;
    [SerializeField] private GameObject _pauseGamePanel;

    [Header("Components: ")]
    [SerializeField] private SettingCameraDemoGame _demoGame;
    private PlayerController _playerController;
    [SerializeField] private Image _imageCoinUI;

    [Header("Condition star win: ")]
    [SerializeField] private int _totalCoin;
    [SerializeField] private float _timeLimit;

    [Header("Shield: ")]
    [SerializeField] private Image _shieldIconUI;
    [SerializeField] private bool _isShieldActive = false;

    [Header("Treasure UI: ")]
    [SerializeField] private TextMeshProUGUI _treasureProgressText;

    [Header("Reward System")]
    [SerializeField] private GameObject _treasurePrefab;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private List<ItemSO> _rewardItems;
    [SerializeField] private int _coinPerTreasure = 10;
    private int _coinProgress = 0;

    private int _score = 0;
    private float _elapseTime;

    private bool _isGameOver = false;
    private bool _isVictory = false;
    private bool _isRunning = true;
    private void Awake()
    {
        _playerController = FindObjectOfType<PlayerController>();
        _demoGame = FindObjectOfType<SettingCameraDemoGame>();
    }
    void Start()
    {
        UpdateScore();
        _gameOverPanel.SetActive(false);
        _victoryPanel.SetActive(false);
        _pauseGamePanel.SetActive(false);
        _shieldIconUI.enabled = false;
        ShowMapName(_nameMapText);
        UpdateTreasureUI();
    }
    private void Update()
    {
        if(!_isGameOver && !_isVictory)
        {
            UpdateTimer();
        }

        RotateImage();
        PauseGamePanel();
    }
    #region Time
    private void UpdateTimer()
    {
        if (_playerController.enabled == false)
            return;
        _elapseTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(_elapseTime / 60);
        int seconds = Mathf.FloorToInt(_elapseTime % 60);
        _timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    public void ResetTimer()
    { 
        _elapseTime = 0;
        UpdateTimer();
    }
    public void StopTimer()
    {
        _isRunning = false;
    }
    public void StartTimer()
    {
        _isRunning = true;    
    }
    #endregion
    #region Score
    public void AddScore(int points)
    {
        if (!_isGameOver && !_isVictory)
        {
            _score += points;
            _coinProgress += points;
            UpdateScore();
            UpdateTreasureUI();
        }
        if(_coinProgress >= _coinPerTreasure)
        {
            _coinProgress -= _coinPerTreasure;
            SpawnTreasure();
            UpdateTreasureUI();
        }
        
    }
    private void UpdateScore()
    {
        _scoreText.text = "Coin: " + _score.ToString();
    }
    private void SpawnTreasure()
    {
        if (_treasurePrefab == null || _playerTransform == null || _rewardItems.Count == 0) return;

        int index = Random.Range(0, _rewardItems.Count);
        ItemSO selectedItem = _rewardItems[index];

        Vector3 spawnPos = _playerTransform.position + new Vector3(0, 2f, 0);
        GameObject treasure = Instantiate(_treasurePrefab, spawnPos, Quaternion.identity);

        TreasureBox treasureBox = treasure.GetComponent<TreasureBox>();
        if (treasureBox != null)
        {
            treasureBox.SetReward(selectedItem);
        }
    }
    private void UpdateTreasureUI()
    {
        int remaining = _coinPerTreasure - _coinProgress;
        _treasureProgressText.text = $"{_coinProgress:D2} / {_coinPerTreasure:D2}";
    }
    #endregion
    #region Image
    private void RotateImage()
    {
        _imageCoinUI.transform.Rotate(0, 180f * Time.deltaTime, 0);
    }
    #endregion
    #region State
    public void StartGame()
    {
        _playerController.enabled = true;
        StartTimer();
    }
    public void GameOver()
    {
        _isGameOver = true;
        _score = 0;
        Time.timeScale = 0;
        _gameOverPanel.SetActive(true);
    }
    public void VictoryGame()
    {
        _isVictory = true;
        Time.timeScale = 0;
        _victoryPanel.SetActive(true);

        _victoryCoinText.text = "Số xu nhặt được: " + _score.ToString();

        int minutes = Mathf.FloorToInt(_elapseTime / 60);
        int seconds = Mathf.FloorToInt(_elapseTime % 60);
        _victoryTimeText.text = "Thời gian qua màn: " + string.Format("{0:00}:{1:00}", minutes, seconds);

        VictoryPanelController controller = FindObjectOfType<VictoryPanelController>();
        if (controller != null)
        {
            int stars = 1;
            if (_score >= _totalCoin) stars++;
            if (_elapseTime <= _timeLimit) stars++;
            controller.ShowStars(stars);
        }
    }
    #endregion
    #region PauseGame
    public void PauseGamePanel()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            _pauseGamePanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }
    public void ResumeGame()
    {
        _pauseGamePanel.SetActive(false);
        Time.timeScale = 1f;
    }
    public void RestartGame()
    {
        _isGameOver = false;
        _score = 0; UpdateScore();
        ResetTimer();
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    #endregion
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;

        if(FindObjectOfType<MovingPlatform>() != null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null && player.transform.parent != null)
            {
                player.transform.SetParent(null);
            }
        }
    }
    public void LoadNextLevel()
    {
        Time.timeScale = 1f;

        int curSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = curSceneIndex + 1;

        if(nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
    public bool IsGameOver()
    {
        return _isGameOver;
    }
    public bool IsVictoryGame()
    {
        return _isVictory;
    }
    #region NameMap
    public void ShowMapName(string mapName)
    {
        _mapNameText.text = mapName;
        _mapNameText.gameObject.SetActive(true);
        StartCoroutine(HideMapNameAfterDelay());

    }
    private IEnumerator HideMapNameAfterDelay()
    {
        yield return new WaitForSeconds(5f);
        _mapNameText.gameObject.SetActive(false);
    }
    #endregion
    #region Shield
    public void ActivatedShield()
    {
        _isShieldActive = true;
        if (_imageCoinUI != null)
        {
            _shieldIconUI.enabled = true;
        }
    }
    public void DeactivatedShield()
    {
        if (_isShieldActive)
        {
            _isShieldActive = false;
            if(_shieldIconUI != null)
                _shieldIconUI.enabled = false;
        }
    }
    public bool IsShieldActive()
    {
        return _isShieldActive;
    }
    #endregion
}
