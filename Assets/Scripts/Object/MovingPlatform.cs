using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform _posA;
    [SerializeField] private Transform _posB;
    [SerializeField] private float _platformSpeed;

    private Vector3 _target;
    private Transform _player;
    void Start()
    {
        _target = _posA.position;
    }
    void Update()
    {
        PlatformMoving();
    }
    private void OnDisable()
    {
        if(_player != null)
        {
            _player.SetParent(null);
            _player = null;
        }
    }
    private void PlatformMoving()
    {
        transform.position = Vector3.MoveTowards(transform.position, _target, _platformSpeed * Time.deltaTime);
        if(Vector3.Distance(transform.position, _target) < 0.1f)
        {
            if(_target == _posA.position) _target = _posB.position;
            else _target = _posA.position;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (gameObject.activeInHierarchy && collision.gameObject.activeInHierarchy)
                StartCoroutine(DetachLater(collision.transform));
        }
    }
    IEnumerator DetachLater(Transform obj)
    {
        yield return null;
        if(obj != null)
            obj.SetParent(null);
    }
}
