using UnityEngine;
using DG.Tweening;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _lifetime = 3f;
    private Tween moveTween;

    [Header("References: ")]
    [SerializeField] private AudioManager _audioManager;

    [Header("Effect:")]
    [SerializeField] private GameObject _explosionEffect;

    private void Awake()
    {
        _audioManager = FindAnyObjectByType<AudioManager>();
    }
    public void Shoot(Vector3 startPos, Vector3 targetPos)
    {
        transform.position = startPos;

        Vector2 dir = (targetPos - startPos).normalized;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        Vector3 endPos = startPos + (Vector3)dir * 20f;

        moveTween?.Kill();
        moveTween = transform.DOMove(
            endPos,
            Vector2.Distance(startPos, endPos) / _speed
        ).SetEase(Ease.Linear)
         .OnComplete(() => DisableBullet());

        CancelInvoke();
        Invoke(nameof(DisableBullet), _lifetime);
    }

    private void DisableBullet()
    {
        moveTween?.Kill();
        gameObject.SetActive(false);
    }
    private void SpawnExplosion()
    {
        if (_explosionEffect != null)
        {
            GameObject explosion = Instantiate(_explosionEffect, transform.position, Quaternion.identity, transform);
            Destroy(explosion, 1.5f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyController enemy = collision.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.Die();
            } 
            DisableBullet();
            SpawnExplosion();
            _audioManager?.PlayExplosionSound();
        }
        else if (collision.CompareTag("Ground"))
        {
            DisableBullet();
            SpawnExplosion();
            _audioManager?.PlayExplosionSound();
        }
    }
}
