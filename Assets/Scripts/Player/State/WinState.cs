using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinState : IPlayerStates
{
    public void Enter(PlayerController player)
    {
        player.animator.Play("Win");
        player.playerRb.velocity = Vector2.zero;
        player.playerRb.gravityScale = 0f;
    }
    public void Update(PlayerController player, float xInput, bool jumpPressed)
    {
    }
    public void Exit(PlayerController player)
    {
        player.playerRb.gravityScale = 4f;
    }

    

    
}
