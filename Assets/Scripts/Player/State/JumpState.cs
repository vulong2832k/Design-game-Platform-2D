using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : IPlayerStates
{
    public void Enter(PlayerController player)
    {
        if (player.currentJumpCount < player.maxJumpCount)
        {
            player.animator.Play("Jump", 0, 0f);
            player.PlayJumpSound();
            player.playerRb.velocity = new Vector2(player.playerRb.velocity.x, player.JumpForce);

            player.currentJumpCount++;
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

        AnimatorStateInfo stateInfo = player.animator.GetCurrentAnimatorStateInfo(0);

        if ((player.IsGrounded || player.IsTrap) && stateInfo.IsName("Jump"))
        {
            player.currentJumpCount = 0;

            if (Mathf.Abs(xInput) > 0.01f)
                player.TransitionToState(new WalkState());
            else
                player.TransitionToState(new IdleState());
        }
        else if (jumpPressed && player.currentJumpCount < player.maxJumpCount)
        {
            player.TransitionToState(new JumpState());
        }
    }
    public void Exit(PlayerController player)
    {
    }
}
