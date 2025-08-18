using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IPlayerStates
{
    public void Enter(PlayerController player)
    {
        player.animator.Play("Idle");
    }
    public void Update(PlayerController player, float xInput, bool jumpPressed)
    {
        if (Input.GetKey(KeyCode.S))
        {
            player.TransitionToState(new CrouchState());
            return;
        }
        if (Mathf.Abs(xInput) > 0.01f)
            player.TransitionToState(new WalkState());

        if (jumpPressed && (player.IsGrounded || player.IsTrap))
        {
            player.TransitionToState(new JumpState());
        }
    }
    public void Exit(PlayerController player)
    {
    }

}
