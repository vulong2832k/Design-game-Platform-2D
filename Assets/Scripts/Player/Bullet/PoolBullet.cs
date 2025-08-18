using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolBullet : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private int _size = 10;

    private Queue<GameObject> _poolBullet = new Queue<GameObject>();

    private void Awake()
    {
        for (int i = 0; i < _size; i++)
        {
            GameObject bullet = Instantiate(_prefab, transform);
            bullet.SetActive(false);
            _poolBullet.Enqueue(bullet);
        }
    }

    public GameObject GetBullet()
    {
        if (_poolBullet.Count > 0)
        {
            GameObject bullet = _poolBullet.Dequeue();
            bullet.SetActive(true);
            return bullet;
        }

        GameObject newBullet = Instantiate(_prefab, transform);
        return newBullet;
    }

    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        _poolBullet.Enqueue(bullet);
    }
}
