using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyUnlockNewLevel : MonoBehaviour
{
    private const string LEVEL_UNLOCK_KEY = "levelUnlocked";

    public int levelToUnlock;

    private int _numberOfUnlockLevels;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _numberOfUnlockLevels = PlayerPrefs.GetInt(LEVEL_UNLOCK_KEY);

            if(_numberOfUnlockLevels < levelToUnlock)
            {
                PlayerPrefs.SetInt(LEVEL_UNLOCK_KEY, _numberOfUnlockLevels + 1);
                PlayerPrefs.Save();
            }
        }
    }
}
