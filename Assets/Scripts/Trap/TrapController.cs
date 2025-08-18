using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapController : MonoBehaviour
{
    [SerializeField] private Transform _posA;
    [SerializeField] private Transform _posB;
    [SerializeField] private float _trapSpeed;

    private Vector3 _target;
    private Transform _player;
    void Start()
    {
        _target = _posA.position;
    }

    void Update()
    {
        TrapMoving();
    }
    private void TrapMoving()
    {
        transform.position = Vector3.MoveTowards(transform.position, _target, _trapSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, _target) < 0.1f)
        {
            if (_target == _posA.position) _target = _posB.position;
            else _target = _posA.position;
        }
    }
}
