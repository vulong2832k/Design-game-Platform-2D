using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxDamageTrapFire : MonoBehaviour
{
    [SerializeField] private int _damage = 1;
    [SerializeField] private float _damageInterval = 2f;

    private List<PlayerController> _players = new List<PlayerController>();
    private Coroutine _damageCoroutine;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null && !_players.Contains(player))
            {
                _players.Add(player);
                if (_damageCoroutine == null)
                    _damageCoroutine = StartCoroutine(DamageLoop());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null && _players.Contains(player))
            {
                _players.Remove(player);
                if (_players.Count == 0 && _damageCoroutine != null)
                {
                    StopCoroutine(_damageCoroutine);
                    _damageCoroutine = null;
                }
            }
        }
    }

    private IEnumerator DamageLoop()
    {
        while (true)
        {
            for (int i = _players.Count - 1; i >= 0; i--)
            {
                try
                {
                    var player = _players[i];
                    if (player != null && player.gameObject.activeInHierarchy)
                    {
                        player.TakeDamage(_damage);
                    }
                    else
                    {
                        _players.RemoveAt(i);
                    }
                }
                catch (System.Exception ex)
                {
                    _players.RemoveAt(i);
                }
            }

            if (_players.Count == 0)
            {
                _damageCoroutine = null;
                yield break;
            }

            yield return new WaitForSeconds(_damageInterval);
        }
    }

}
