using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BarUI : MonoBehaviour
{
    [SerializeField] private Image _hpBar;
    [SerializeField] private Image _manaBar;

    [SerializeField] private PlayerController _playerController;
    private void Awake()
    {
        _playerController = FindObjectOfType<PlayerController>();
    }
    public void UpdateHpBarUI()
    {
        _hpBar.fillAmount = (float)_playerController.currentHp / _playerController.maxHp;
    }
    public void UpdateManaBarUI()
    {
        _manaBar.fillAmount = (float)_playerController.currentMana / _playerController.maxMana;
    }
}
