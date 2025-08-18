using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeslaController : MonoBehaviour
{
    [SerializeField] private GameObject _bullet;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private float _fireMinDelay = 3;
    [SerializeField] private float _fireMaxDelay = 5;


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
        Instantiate(_bullet, _firePoint.position, Quaternion.identity);
    }
}
