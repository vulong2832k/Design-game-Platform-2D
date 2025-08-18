using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchSprites : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _switchVisual;
    [SerializeField] private Sprite _activatedSprite;

    private bool _isTriggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isTriggered || !collision.CompareTag("Player")) return;

        if (_switchVisual != null && _activatedSprite != null)
        {
            _switchVisual.sprite = _activatedSprite;
        }

        _isTriggered = true;
    }
}
