using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchState : IPlayerStates
{
    public void Enter(PlayerController player)
    {
        player.animator.Play("Crouch");

        player.playerRb.velocity = new Vector2 (0, player.playerRb.velocity.y);
    }
    public void Update(PlayerController player, float xInput, bool jumpPressed)
    {
        if (!Input.GetKey(KeyCode.S))
        {
            if(Mathf.Abs(xInput) < 0.1f)
            {
                player.TransitionToState(new IdleState());
            }
            else
            {
                player.TransitionToState(new WalkState());
            }
        }
    }
    public void Exit(PlayerController player)
    {

    }

}
