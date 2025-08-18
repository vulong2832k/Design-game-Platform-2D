using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine
{
    private IEnemyState _currentState;

    public void ChangeState(IEnemyState newState, EnemyController enemy)
    {
        _currentState?.Exit(enemy);
        _currentState = newState;
        _currentState.Enter(enemy);
    }
    public void UpdateState(EnemyController enemy)
    {
        _currentState?.Update(enemy);
    }
    public IEnemyState GetCurrentState()
    {
        return _currentState;
    }
}
