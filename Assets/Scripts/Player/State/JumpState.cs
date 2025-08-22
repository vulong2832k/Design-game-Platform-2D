using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : IPlayerStates
{
    public void Enter(PlayerController player)
    {
        if (!player.hasAppliedJumpForce && player.currentJumpCount < player.maxJumpCount)
        {
            player.animator.Play("Jump", 0, 0f);
            player.PlayJumpSound();
            player.playerRb.velocity = new Vector2(player.playerRb.velocity.x, player.JumpForce);

            player.currentJumpCount++;
            player.hasAppliedJumpForce = true;
        }
        else
        {
            player.animator.Play("Jump");
        }
    }

    public void Update(PlayerController player, float xInput, bool jumpPressed)
    {
        player.PlayerMovement(xInput);

        if (Input.GetKey(KeyCode.W) && player.currentJumpCount == player.maxJumpCount)
        {
            player.TransitionToState(new StrikeState());
            return;
        }

        if ((player.IsGrounded || player.IsTrap) && player.playerRb.velocity.y <= 0f)
        {
            player.currentJumpCount = 0;
            player.hasAppliedJumpForce = false;

            if (Mathf.Abs(xInput) > 0.01f)
                player.TransitionToState(new WalkState());
            else
                player.TransitionToState(new IdleState());
        }
        else if (jumpPressed && player.currentJumpCount < player.maxJumpCount)
        {
            player.hasAppliedJumpForce = false;
            player.TransitionToState(new JumpState());
        }
    }

    public void Exit(PlayerController player)
    {
    }
}
