using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolTesla : MonoBehaviour
{
    [SerializeField] private GameObject _teslaPrefab;
    [SerializeField] private int _poolSize = 10;

    private Queue<GameObject> _pool = new Queue<GameObject>();

    void Start()
    {
        for (int i = 0; i < _poolSize; i++)
        {
            GameObject tesla = Instantiate(_teslaPrefab, transform);
            tesla.SetActive(false);
            _pool.Enqueue(tesla);
        }
    }

    public GameObject GetFromPool(Vector3 position, Quaternion rotation)
    {
        if (_pool.Count > 0)
        {
            GameObject tesla = _pool.Dequeue();
            tesla.SetActive(true);
            tesla.transform.position = position;
            tesla.transform.rotation = rotation;
            return tesla;
        }
        else
        {
            GameObject tesla = Instantiate(_teslaPrefab, position, rotation, transform);
            return tesla;
        }
    }

    public void ReturnToPool(GameObject tesla)
    {
        tesla.SetActive(false);
        _pool.Enqueue(tesla);
    }
}
