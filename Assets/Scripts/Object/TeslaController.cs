using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeslaController : MonoBehaviour
{
    [SerializeField] private PoolTesla _bulletPool;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private float _fireMinDelay = 3;
    [SerializeField] private float _fireMaxDelay = 5;

    private void Awake()
    {
        _bulletPool = FindAnyObjectByType<PoolTesla>();
    }
    void Start()
    {
        StartCoroutine(ShootBulletCoroutine());
    }

    private IEnumerator ShootBulletCoroutine()
    {
        while (true)
        {
            SpawnBullet();
            float randomDelay = Random.Range(_fireMinDelay, _fireMaxDelay);
            yield return new WaitForSeconds(randomDelay);
        }
    }

    private void SpawnBullet()
    {
        GameObject bulletObj = _bulletPool.GetFromPool(_firePoint.position, Quaternion.identity);
        bulletObj.GetComponent<BulletTesla>().Init(_bulletPool);
    }
}

