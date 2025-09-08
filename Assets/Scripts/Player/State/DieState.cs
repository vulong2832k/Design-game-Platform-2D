using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DieState : IPlayerStates
{
    private bool _hasPlayedDeath = false;
    public void Enter(PlayerController player)
    {
        if (_hasPlayedDeath) return;
        _hasPlayedDeath = true;

        player.animator.Play("Die");
        player.playerRb.bodyType = RigidbodyType2D.Kinematic;
        player.playerRb.velocity = Vector2.zero;
        player.GetComponent<SpriteRenderer>().DOFade(0f, 1.5f).OnComplete(() =>
        {
            player._audioManager?.PlayDeathSound();
            player._gameManager.GameOver();
        });
    }

    public void Update(PlayerController player, float xInput, bool jumpPressed)
    {
    }

    public void Exit(PlayerController player)
    {
    }
}
