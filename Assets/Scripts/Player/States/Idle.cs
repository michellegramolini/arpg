using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : State
{
    private PlayerController _player;

    public override void FixedUpdateState(PlayerController player)
    {
        StopMovement();
    }

    public override void StartState(PlayerController player)
    {
        this._player = player;

        Animate();
    }

    public override void UpdateState(PlayerController player)
    {
        if (_player.moveVector != _player.idleVector)
        {
            _player.SetState(_player.Walk);
        }

        if (_player.isJumping)
        {
            _player.SetState(_player.Jump);
        }

        if (_player.canSwim)
        {
            _player.SetState(_player.Swim);
        }

        if (_player.isAttacking)
        {
            _player.SetState(_player.MeleeAttack);
        }
    }

    public override void LateUpdateState(PlayerController player)
    {
        Animate();
    }

    private void StopMovement()
    {
        _player.rb.velocity = Vector2.zero;
    }

    private void Animate()
    {
        // use facing direction and set facing animation
        if (_player.facingDirection == Vector2.up)
        {
            _player.animationState.SetAnimationState("player_idle_up");
        }
        else if (_player.facingDirection == Vector2.down)
        {
            _player.animationState.SetAnimationState("player_idle_down");
        }
        // TODO: could handle this at the facingDirection vector level
        else if (_player.facingDirection.x > 0f)
        {
            _player.animationState.SetAnimationState("player_idle_right");
        }
        else
        {
            _player.animationState.SetAnimationState("player_idle_left");
        }
    }

}
