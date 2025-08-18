using UnityEngine;

public class WindFan : MonoBehaviour
{
    [SerializeField] private float _windForce;
    [SerializeField] private bool _useUpDirection = true;
    [SerializeField] private float _maxWindSpeed;

    private Rigidbody2D _playerRb;
    private bool _isInWindZone = false;
    private Vector2 _windDir;

    private void FixedUpdate()
    {
        if (_isInWindZone && _playerRb != null)
        {
            // Apply gió
            _playerRb.AddForce(_windDir * _windForce, ForceMode2D.Force);

            // Giới hạn tốc độ theo hướng gió
            float speedInWindDir = Vector2.Dot(_playerRb.velocity, _windDir);
            if (speedInWindDir > _maxWindSpeed)
            {
                Vector2 cappedVelocity = _windDir * _maxWindSpeed;
                Vector2 velocityPerpendicular = _playerRb.velocity - _windDir * speedInWindDir;
                _playerRb.velocity = cappedVelocity + velocityPerpendicular;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _playerRb = collision.GetComponent<Rigidbody2D>();
            _isInWindZone = true;
            _windDir = (_useUpDirection ? transform.up : transform.right).normalized;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (_playerRb != null)
            {
                // Khi ra khỏi gió: giảm tốc theo hướng gió (nhẹ nhàng dừng lại)
                float speedInWindDir = Vector2.Dot(_playerRb.velocity, _windDir);
                Vector2 velocityPerpendicular = _playerRb.velocity - _windDir * speedInWindDir;
                _playerRb.velocity = velocityPerpendicular;
            }

            _playerRb = null;
            _isInWindZone = false;
        }
    }
}
