using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private AudioManager _audioManager;
    [SerializeField] private PlayerController _playerController;

    [Header("Effects: ")]
    [SerializeField] private GameObject _bloodEffectPrefab;
    [SerializeField] private GameObject _victoryEffectPrefab;
    [SerializeField] private GameObject _coinEffectPrefab;

    private void Awake()
    {
        _gameManager = FindAnyObjectByType<GameManager>();
        _audioManager = FindAnyObjectByType<AudioManager>();
    }
    private void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();
    }
    public void SpawnBlood()
    {
        if (_bloodEffectPrefab != null && _playerController != null)
        {
            GameObject blood = Instantiate(_bloodEffectPrefab, transform.position, Quaternion.identity, transform);
            Destroy(blood, 1.5f);
        }
    }
    private void SpawnCoinEffect()
    {
        if(_coinEffectPrefab != null && _playerController != null)
        {
            GameObject coinEffect = Instantiate(_coinEffectPrefab, transform.position, Quaternion.identity);
            Destroy(coinEffect, 1f);
        }
    }
    private void SpawnVictoryEffect()
    {
        if (_victoryEffectPrefab != null && _playerController != null)
        {
            GameObject victoryEffect = Instantiate(_victoryEffectPrefab, transform.position, Quaternion.identity);
            Destroy(victoryEffect, 2f);
        }
    }
    private IEnumerator DelayVictory()
    {
        yield return new WaitForSeconds(2f);
        _gameManager?.VictoryGame();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            _playerController.TakeDamage(1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            Destroy(collision.gameObject);
            SpawnCoinEffect();
            _audioManager.PlayCoinSound();
            _gameManager.AddScore(1);
        }
        else if (collision.gameObject.CompareTag("Key"))
        {
            Destroy(collision.gameObject);
            _playerController.OnWinTrigger();
            _audioManager?.PlayWinSound();
            SpawnVictoryEffect();
            StartCoroutine(DelayVictory());
            
        }
        else if(collision.CompareTag("The Void"))
        {
            _playerController.TakeDamage(1000);
        }
        
    }
}
