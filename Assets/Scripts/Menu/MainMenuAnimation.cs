using System.Collections;
using UnityEngine;

public class MainMenuAnimation : MonoBehaviour
{
    [SerializeField] private Transform _playerStartPoint;
    [SerializeField] private Transform _playerEndPoint;

    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _jumpInterval;

    private Rigidbody2D _rb;
    private Animator _anim;

    private bool _movingToEnd = true;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        StartCoroutine(JumpRoutine());
    }

    void Update()
    {
        PlayerMoving();

        // set animation speed
        float horizontalSpeed = (_movingToEnd ? 1 : 1) * _moveSpeed;
        _anim.SetFloat("MenuSpeed", Mathf.Abs(horizontalSpeed));

        // set animation jump
        _anim.SetBool("MenuIsJumping", !IsGrounded());
    }

    private void PlayerMoving()
    {
        Vector3 target = _movingToEnd ? _playerEndPoint.position : _playerStartPoint.position;

        transform.position = Vector3.MoveTowards(transform.position, target, _moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            _movingToEnd = !_movingToEnd;

            if (!_movingToEnd)
                StartCoroutine(TeleportToStartPoint());
        }
    }

    private IEnumerator JumpRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_jumpInterval);

            if (IsGrounded())
            {
                _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);
            }
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, 0.1f);
    }

    private IEnumerator TeleportToStartPoint()
    {
        yield return new WaitForSeconds(0.5f);
        transform.position = _playerStartPoint.position;
    }
}
