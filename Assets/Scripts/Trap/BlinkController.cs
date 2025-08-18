using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private int _damage;
    [SerializeField] private Transform[] _listMoving;

    private Animator _anim;
    private Rigidbody2D _rb;

    private int _currentIndex = 1;

    private bool _isHitting = false;
    private float _hitDuration = 0.3f;
    private float _hitTimer = 0f;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();

        if (_listMoving.Length > 0)
            transform.position = _listMoving[0].position;
    }

    void Update()
    {
        if (_isHitting)
        {
            _hitTimer -= Time.deltaTime;
            if (_hitTimer <= 0f)
            {
                _isHitting = false;
            }
        }
        else
        {
            MoveToTarget();
        }
    }
    private void MoveToTarget()
    {
        if (_listMoving.Length == 0  || _isHitting) return;

        Vector2 target = _listMoving[_currentIndex].position;
        Vector2 dir = (target - (Vector2)transform.position).normalized;

        _rb.MovePosition(_rb.position + dir * _moveSpeed * Time.deltaTime);

        _anim.Play("BlinkMove");

        if (Vector2.Distance(transform.position, target) < 0.1f)
        {
            if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            {
                if (dir.x > 0)
                    _anim.Play("BlinkRightHit");
                else
                    _anim.Play("BlinkLeftHit");
            }
            else
            {
                if (dir.y > 0)
                    _anim.Play("BlinkTopHit");
                else
                    _anim.Play("BlinkBottomHit");
            }
            _isHitting = true;
            _hitTimer = _hitDuration;

            _currentIndex = (_currentIndex + 1) % _listMoving.Length;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
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
