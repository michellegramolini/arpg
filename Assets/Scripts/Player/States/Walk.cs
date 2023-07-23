using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Walk : State
{
    private PlayerController _player;

    public override void FixedUpdateState(PlayerController player)
    {
        MovePlayer();
    }

    public override void StartState(PlayerController player)
    {
        this._player = player;

        Animate();
    }

    public override void UpdateState(PlayerController player)
    {
        if (_player.moveVector == _player.idleVector)
        {
            _player.SetState(_player.Idle);
        }

        //Animate();

        if (_player.isShifting)
        {
            _player.SetState(_player.Run);
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

    // TODO: DRY
    private void MovePlayer()
    {
        if (_player.canMove)
        {
            _player.rb.velocity = _player.walkSpeed * _player.moveVector;
        }
        // else stop movement?
        else
        {
            _player.rb.velocity = Vector2.zero;
        }
    }

    private void Animate()
    {
        if (_player.moveVector.x > 0f)
        {
            _player.animationState.SetAnimationState("player_walk_right");
        }
        else if (_player.moveVector.x < 0f)
        {
            _player.animationState.SetAnimationState("player_walk_left");
        }
        else if (_player.moveVector.x == 0f && _player.moveVector.y > 0f)
        {
            _player.animationState.SetAnimationState("player_walk_up");
        }
        else
        {
            _player.animationState.SetAnimationState("player_walk_down");
        }
    }

}
