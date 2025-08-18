using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUnlockHandler : MonoBehaviour
{
    private const string LEVEL_UNLOCK_KEY = "levelUnlocked"; 

    [SerializeField] private Button[] _listLevelBtn;

    private int _unlockLevelsNumber;

    void Start()
    {
        UnlockFirstLevel();
        UnlockLevelsNumber();
    }
    private void UnlockFirstLevel()
    {
        if (!PlayerPrefs.HasKey(LEVEL_UNLOCK_KEY))
        {
            PlayerPrefs.SetInt(LEVEL_UNLOCK_KEY, 1);
            PlayerPrefs.Save();
        }
        _unlockLevelsNumber = PlayerPrefs.GetInt(LEVEL_UNLOCK_KEY);

        for (int i = 0; i < _listLevelBtn.Length; i++)
        {
            _listLevelBtn[i].interactable = false;
        }
    }
    private void UnlockLevelsNumber()
    {
        _unlockLevelsNumber = PlayerPrefs.GetInt(LEVEL_UNLOCK_KEY);

        for (int i = 0; i < _unlockLevelsNumber; i++)
        {
            _listLevelBtn[i].interactable = true;
        }
    }
}
