using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class TrapFire : MonoBehaviour
{
    [Header("Trap Timing")]
    [SerializeField] private float _warningDelay = 1f;
    [SerializeField] private float _fireDuration = 2f;

    [Header("References")]
    [SerializeField] private Animator _animTrap;
    [SerializeField] private BoxCollider2D _boxDamage;

    private Coroutine _trapCoroutine;

    void Start()
    {
        _animTrap.Play("Idle");
        _boxDamage.enabled = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_trapCoroutine == null && collision.gameObject.CompareTag("Player"))
        {
            _trapCoroutine = StartCoroutine(TriggerTrap());
        }
    }

    private IEnumerator TriggerTrap()
    {
        _animTrap.Play("Warning");
        yield return new WaitForSeconds(_warningDelay);

        _animTrap.Play("Fire");
        _boxDamage.enabled = true;

        yield return new WaitForSeconds(_fireDuration);

        _boxDamage.enabled = false;
        _animTrap.Play("Idle");

        _trapCoroutine = null;
    }

}
