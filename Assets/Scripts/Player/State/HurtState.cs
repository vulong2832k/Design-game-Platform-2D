using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtState : IPlayerStates
{
    private float _hurtDuration = 1f;
    private float _timer;

    public void Enter(PlayerController player)
    {
        _timer = _hurtDuration;
        player.animator.Play("Hurt");

        ApplyKnockBack(player);
        player.StartCoroutine(EnableInvincible(player, _hurtDuration));
    }

    public void Update(PlayerController player, float xInput, bool jumpPressed)
    {
        _timer -= Time.deltaTime;

        if (_timer <= 0)
        {
            if (player.IsDied)
            {
                player.TransitionToState(new DieState());
            }
            else
            {
                player.TransitionToState(new IdleState());
            }
        }
    }
    public void Exit(PlayerController player)
    {

    }

    private IEnumerator EnableInvincible(PlayerController player, float duration)
    {
        player.SetInvincible(true);
        yield return new WaitForSeconds(duration);
        player.SetInvincible(false);
    }
    private void ApplyKnockBack(PlayerController player)
    {
        float knockBackDir = player.transform.localScale.x > 0 ? -1 : 1;

        Vector2 force = new Vector2(knockBackDir * 5f, 2f);
        player.playerRb.velocity = Vector2.zero;
        player.playerRb.AddForce(force, ForceMode2D.Impulse);
    }
}
