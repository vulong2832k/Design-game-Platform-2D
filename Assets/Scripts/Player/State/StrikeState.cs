using DG.Tweening;
using UnityEngine;

public class StrikeState : IPlayerStates
{
    private float _originalGravity;
    private Tween _gravityTween;

    public void Enter(PlayerController player)
    {
        player.animator.Play("Strike");

        _originalGravity = player.playerRb.gravityScale;

        if (player.playerRb.velocity.y > 0)
            player.playerRb.velocity = new Vector2(player.playerRb.velocity.x, 0f);

        player.playerRb.gravityScale = 0.3f;
    }

    public void Update(PlayerController player, float xInput, bool strikeHeld)
    {
        AnimatorStateInfo stateInfo = player.animator.GetCurrentAnimatorStateInfo(0);

        if (Input.GetKey(KeyCode.W))
            return;

        if (strikeHeld)
        {
            if (!stateInfo.IsName("Strike"))
            {
                player.animator.Play("Strike", 0, 0f);
            }
            player.playerRb.gravityScale = 0.3f;
            return;
        }
        else
        {
            if (player.playerRb.gravityScale != _originalGravity)
            {
                _gravityTween?.Kill();

                _gravityTween = DOTween.To(
                    () => player.playerRb.gravityScale,
                    x => player.playerRb.gravityScale = x,
                    _originalGravity, 0.3f).SetEase(Ease.OutQuad);
            }

            if (stateInfo.IsName("Strike"))
            {
                if (player.IsGrounded)
                {
                    player.playerRb.velocity = Vector2.zero;
                    player.TransitionToState(new IdleState());
                }
            }
        }
    }

    public void Exit(PlayerController player)
    {
        _gravityTween?.Kill();
        player.playerRb.gravityScale = _originalGravity;
    }
}
