using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float _enemySpeed;
    [SerializeField] private float _enemyDistance;
    [SerializeField] private float _distanceAttack;
    [SerializeField] private EnemyType _enemyType;
    private Vector3 _startPos;
    private bool _movingRight = true;
    public bool CanMove = true;
    public EnemyType EnemyType { get => _enemyType; }

    private IEnemyState _currentState;

    [Header("Effect: ")]
    [SerializeField] private GameObject _dieEffect;

    public Animator Animator { get; private set; }
    public Transform Player { get; private set; }
    public EnemyStateMachine StateMachine { get; private set; }
    public IEnemyAttack EnemyAttack { get; private set; }


    void Start()
    {
        _startPos = transform.position;

        Animator = GetComponent<Animator>();

        Player = GameObject.FindGameObjectWithTag("Player").transform;

        switch (EnemyType)
        {
            case EnemyType.Shooter:
                EnemyAttack = GetComponent<EnemyShootAttack>();
                break;
            case EnemyType.Moving:
                EnemyAttack = GetComponent<EnemyNoAttack>();
                break;
            case EnemyType.Droper:
                EnemyAttack = GetComponent<EnemyDropBom>();
                break;
            case EnemyType.Heavy:
                EnemyAttack = GetComponent<EnemyHeavyAttack>();
                break;
        }

        StateMachine = new EnemyStateMachine();
        StateMachine.ChangeState(new EnemyStates.IdleState(), this);
    }

    void Update()
    {
        EnemyMoving();

        StateMachine.UpdateState(this);
    }
    public void EnemyMoving()
    {
        if(!CanMove) return;

        float leftBound = _startPos.x - _enemyDistance;
        float rightBound = _startPos.x + _enemyDistance;
        if(_movingRight)
        {
            transform.Translate(Vector2.right * _enemySpeed * Time.deltaTime);
            if(transform.position.x >= rightBound)
            {
                _movingRight = false;
                FlipEnemy();
            } 
        }
        else
        {
            transform.Translate(Vector2.left * _enemySpeed * Time.deltaTime);
            if (transform.position.x <= leftBound)
            {
                _movingRight = true;
                FlipEnemy();
            }

        }
    }
    private void FlipEnemy()
    {
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }
    public bool CanAttackPlayer()
    {
        if (Player == null) return false;
        float distance = Vector2.Distance(transform.position, Player.position);
        return distance < _distanceAttack;
    }
    public void ChangeToAttack()
    {
        StateMachine.ChangeState(new EnemyStates.AttackState(), this);
    }
    public void Die()
    {
        SpawnDieEffect();
        gameObject.SetActive(false);
    }
    #region Effect
    private void SpawnDieEffect()
    {
        if (_dieEffect != null)
        {
            GameObject dieEffect = Instantiate(_dieEffect, transform.position, Quaternion.identity);
            Destroy(dieEffect, 2f);
        }
    }
    #endregion
    private void OnDrawGizmosSelected()
    {
        Vector3 leftPoint = transform.position + Vector3.left * _enemyDistance;
        Vector3 rightPoint = transform.position + Vector3.right * _enemyDistance;

        Gizmos.DrawLine(leftPoint, rightPoint);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _distanceAttack);
    }

}
