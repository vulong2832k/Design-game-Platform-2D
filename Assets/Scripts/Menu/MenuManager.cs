using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenuPanel;
    [SerializeField] private GameObject _settingPanel;
    [SerializeField] private GameObject _selectLevelPanel;

    private void Start()
    {
        _mainMenuPanel.SetActive(true);
        _settingPanel.SetActive(false);
        _selectLevelPanel.SetActive(false);
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("GamePlay 1");
        Time.timeScale = 1;
    }
    public void SettingBtn()
    {
        _settingPanel.SetActive(true);
    }
    public void SelectLevelPanel()
    {
        _selectLevelPanel.SetActive(true);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void ExitAudioPanelBtn()
    {
        _settingPanel.SetActive(false);
    }
    public void ExitSelectLevelPanel()
    {
        _selectLevelPanel.SetActive(false);
    }
    public void ResetAllData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
