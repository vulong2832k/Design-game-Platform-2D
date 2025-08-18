using UnityEngine;
using DG.Tweening;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _lifetime = 3f;
    private Tween moveTween;

    public void Shoot(Vector3 startPos, Vector3 targetPos)
    {
        transform.position = startPos;

        Vector2 dir = (targetPos - startPos).normalized;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        Vector3 endPos = startPos + (Vector3)dir * 20f;

        moveTween?.Kill();
        moveTween = transform.DOMove(
            endPos,
            Vector2.Distance(startPos, endPos) / _speed
        ).SetEase(Ease.Linear)
         .OnComplete(() => DisableBullet());

        CancelInvoke();
        Invoke(nameof(DisableBullet), _lifetime);
    }

    private void DisableBullet()
    {
        moveTween?.Kill();
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
            DisableBullet();
        }
        else if (collision.CompareTag("Ground"))
        {
            DisableBullet();
        }
    }
}
