using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTesla : MonoBehaviour
{
    [SerializeField] private float _lifeTime = 2f;
    [SerializeField] private int _damage = 2;
    [SerializeField] private GameObject _hitEffectPrefab;
    [SerializeField] private Transform _spawnEffect;

    void Start()
    {
        SpawnEffectHit();
        Destroy(gameObject, _lifeTime);
    }
    private void SpawnEffectHit()
    {
        if (_hitEffectPrefab != null)
        {
            GameObject effect = Instantiate(_hitEffectPrefab, _spawnEffect.position, Quaternion.identity);
            Destroy(effect, _lifeTime);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(_damage);
            }
        }
    }
}
