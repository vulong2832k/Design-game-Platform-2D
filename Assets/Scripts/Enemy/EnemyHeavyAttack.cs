using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class EnemyHeavyAttack : MonoBehaviour, IEnemyAttack
{
    [SerializeField] private float _attackDelay = 0.5f;
    [SerializeField] private GameObject _hitboxPrefab;
    [SerializeField] private Vector2 _hitboxOffset;
    [SerializeField] private float _hitboxActiveTime = 0.2f;

    private bool _isAttacking = false;
    private Transform _enemy;
    private Transform _player;


    public void Attack(Transform enemy, Transform player)
    {
        if (_isAttacking) return;

        this._enemy = enemy;
        this._player = player;

        enemy.GetComponent<MonoBehaviour>().StartCoroutine(DelayedAttack());
    }

    private IEnumerator DelayedAttack()
    {
        _isAttacking = true;

        yield return new WaitForSeconds(_attackDelay);
        _enemy.GetComponent<Animator>().Play("HeavyAttack");

        yield return new WaitForSeconds(1f);
        _isAttacking = false;
    }
    private void SpawnHitBoxAttack()
    {
        Vector3 spawnPos = _enemy.position + (Vector3)_hitboxOffset * _enemy.localScale.x;
        GameObject hitboxAttack = Instantiate(_hitboxPrefab, spawnPos, Quaternion.identity);
        hitboxAttack.transform.parent = _enemy;

        hitboxAttack.transform.localScale = new Vector3(_enemy.localScale.x, 1, 1);

        Destroy(hitboxAttack, _hitboxActiveTime);
    }
    public void DoDamage()
    {
        if (_player != null && Vector2.Distance(_enemy.position, _player.position) < 2f)
        {
            PlayerController playerCtrl = _player.GetComponent<PlayerController>();
            if (playerCtrl != null)
            {
                playerCtrl.TakeDamage(1);
                Debug.Log("Heavy attack hit player!");
            }
        }
    }
}
