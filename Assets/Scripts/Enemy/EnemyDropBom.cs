using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDropBom : MonoBehaviour, IEnemyAttack
{
    [SerializeField] private GameObject _bombPrefab;
    [SerializeField] private float _cooldown = 2f;
    [SerializeField] private float _rayDistance = 10f;
    private float _lastDropTime = -999f;

    public void Attack(Transform enemy, Transform player)
    {
        if (Time.time - _lastDropTime < _cooldown) return;
        if (player.position.y > enemy.position.y - 0.5f) return;

        RaycastHit2D hit = Physics2D.Raycast(enemy.position, Vector2.down, _rayDistance, LayerMask.GetMask("Player"));
        Debug.DrawRay(enemy.position, Vector2.down * _rayDistance, Color.red);//Vẽ tia của Designer

        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            GameObject bomb = Object.Instantiate(_bombPrefab, enemy.position, Quaternion.identity);
            _lastDropTime = Time.time;
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * _rayDistance);
    }

}
