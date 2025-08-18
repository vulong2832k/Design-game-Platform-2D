using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStates : MonoBehaviour
{
    public class IdleState : IEnemyState
    {
        public void Enter(EnemyController enemy)
        {
            try
            {
                enemy.Animator.Play("Idle");
            }
            catch (System.Exception bug)
            {
            }
        }

        public void Update(EnemyController enemy)
        {
            if (enemy.CanAttackPlayer())
            {
                enemy.StateMachine.ChangeState(new AttackState(), enemy);
            }
        }
        public void Exit(EnemyController enemy)
        {

        }

    }
    public class RunState : IEnemyState
    {
        public void Enter(EnemyController enemy)
        {
            enemy.Animator.Play("Run");
        }

        public void Update(EnemyController enemy)
        {
            enemy.EnemyMoving();

            if (enemy.CanAttackPlayer())
            {
                enemy.StateMachine.ChangeState(new AttackState(), enemy);
            }
        }
        public void Exit(EnemyController enemy)
        {
        }

    }
    public class JumpState : IEnemyState
    {
        public void Enter(EnemyController enemy)
        {
            enemy.Animator.Play("Jump");
        }

        public void Update(EnemyController enemy)
        {
            
        }
        public void Exit(EnemyController enemy)
        {
        }

    }
    public class AttackState : IEnemyState
    {
        private float _attackDuration = 1.5f;
        private float _startTime;
        private bool _hasAttacked = false;

        public void Enter(EnemyController enemy)
        {
            enemy.Animator.Play("Attack");
            _startTime = Time.time;

            if (enemy.EnemyType == EnemyType.Heavy)
                enemy.CanMove = false;
        }

        public void Update(EnemyController enemy)
        {
            enemy.EnemyAttack?.Attack(enemy.transform, enemy.Player);

            if (Time.time - _startTime >= _attackDuration)
            {
                switch (enemy.EnemyType)
                {
                    case EnemyType.Moving:
                        break;
                    case EnemyType.Shooter:
                        enemy.StateMachine.ChangeState(new RunState(), enemy);
                        break;
                    case EnemyType.Droper:
                        enemy.StateMachine.ChangeState(new RunState(), enemy);
                        break;
                    case EnemyType.Heavy:
                        enemy.StateMachine.ChangeState(new RunState(), enemy);
                        break;
                }
            }
        }
        public void Exit(EnemyController enemy)
        {
            if (enemy.EnemyType == EnemyType.Heavy)
                enemy.CanMove = true;
        }

    }

    public class DeadState : IEnemyState
    {
        public void Enter(EnemyController enemy)
        {
            enemy.Animator.Play("Dead");
            enemy.enabled = false;
        }

        public void Update(EnemyController enemy)
        {
        }
        public void Exit(EnemyController enemy)
        {
        }

    }
}
