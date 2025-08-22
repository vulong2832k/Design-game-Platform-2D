using UnityEngine;

public class AttackState : IPlayerStates
{
    private string _triggerName;
    private IPlayerStates _previousState;

    public AttackState(string triggerName, IPlayerStates previousState)
    {
        _triggerName = triggerName;
        _previousState = previousState;
    }

    public void Enter(PlayerController player)
    {
        player.animator.SetTrigger(_triggerName);
    }

    public void Exit(PlayerController player) { }

    public void Update(PlayerController player, float xInput, bool jumpPressed)
    {
    }

    public void BackToPreviousState(PlayerController player)
    {
        player.TransitionToState(_previousState);
    }
}
