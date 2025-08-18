using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [SerializeField] private float _forceBoune;
    [SerializeField] private Animator _anim;

    private Transform _targetPlayer;

    private void Start()
    {
        _anim = GetComponent<Animator>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _targetPlayer = collision.transform;
            _anim.SetTrigger("Bounce");
        }
    }
    public void ApplyBouce()
    {
        if (_targetPlayer != null)
        {
            Rigidbody2D rb = _targetPlayer.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0f);
                rb.AddForce(Vector2.up * _forceBoune, ForceMode2D.Impulse);
            }
            _targetPlayer = null;
        }
    }
}
