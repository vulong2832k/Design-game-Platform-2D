using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : IPlayerStates
{
    public void Enter(PlayerController player)
    {
        player.animator.Play("Walk");
    }
    public void Update(PlayerController player, float xInput, bool jumpPressed)
    {
        if (Input.GetKey(KeyCode.S))
        {
            player.TransitionToState(new CrouchState());
            return;
        }

        player.PlayerMovement(xInput);

        if (Mathf.Abs(xInput) < 0.01f)
            player.TransitionToState(new IdleState());

        if (jumpPressed && (player.IsGrounded || player.IsTrap))
        {
            player.TransitionToState(new JumpState());
        }
    }
    public void Exit(PlayerController player)
    {
    }

}
