using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootAttack : MonoBehaviour, IEnemyAttack
{
    [SerializeField] private float _speedBullet;
    [SerializeField] private GameObject _shootPrefab;
    [SerializeField] private float _cooldown;
    private float _lastShootTimer = -999f;
    public void Attack(Transform enemy, Transform player)
    {
        if(Time.time -_lastShootTimer > _cooldown)
        {
            Vector3 dir = (player.position - enemy.position).normalized;
            dir.z = 0;
            GameObject bullet = Object.Instantiate(_shootPrefab, enemy.position, Quaternion.identity);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = dir * _speedBullet;
            }

            Destroy(bullet, 5f);
            _lastShootTimer = Time.time;
        }
    }
}
