using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenQuestUI : MonoBehaviour
{
    [SerializeField] private GameObject _questUIPanel;

    void Start()
    {
        _questUIPanel.SetActive(false);
    }

    void Update()
    {
        CloseQuestPanel();
    }
    private void CloseQuestPanel()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _questUIPanel.SetActive(false);
            gameObject.SetActive(false);
            Time.timeScale = 1f;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _questUIPanel.SetActive(true);
            Time.timeScale = 0f;
            
        }
    }
}
