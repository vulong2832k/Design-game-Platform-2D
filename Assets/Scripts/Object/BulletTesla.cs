using System.Collections;
using UnityEngine;

public class BulletTesla : MonoBehaviour
{
    private PoolTesla _poolTesla;
    [SerializeField] private float _lifeTime = 2f;
    [SerializeField] private int _damage = 2;
    [SerializeField] private ParticleSystem _hitEffect;
    private float _timer;

    public void Init(PoolTesla pool)
    {
        _poolTesla = pool;
    }

    private void OnEnable()
    {
        _timer = 0f;
        Invoke(nameof(DisableBullet), _lifeTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(_damage);
            }
            if (_hitEffect != null)
            {
                _hitEffect.transform.position = transform.position;
                _hitEffect.Play();
            }
            DisableBullet();
        }
    }

    private void DisableBullet()
    {
        if (_poolTesla != null)
        {
            _poolTesla.ReturnToPool(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}

