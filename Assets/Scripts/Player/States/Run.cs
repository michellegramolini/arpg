using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Run : State
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

        // this could be fun for status effects or something
        //player.animationState.animator.speed *= 2f;
    }

    public override void UpdateState(PlayerController player)
    {
        if (_player.moveVector != _player.idleVector)
        {
            if (!_player.isShifting)
            {
                _player.SetState(_player.Walk);
            }
        }
        else
        {
            _player.SetState(_player.Idle);
        }

        if (_player.canSwim)
        {
            _player.SetState(_player.Swim);
        }

        if (_player.isAttacking)
        {
            _player.SetState(_player.MeleeAttack);
        }

        // TODO: Leap
        if (_player.isJumping)
        {
            _player.SetState(_player.Jump);
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
            _player.rb.velocity = _player.runSpeed * _player.moveVector;
        }
        // else stop movement?
        else
        {
            _player.rb.velocity = Vector2.zero;
        }
    }

    private void Animate()
    {
        if (_player.wasMovingRight)
        {
            if (_player.isMovingRight || _player.isMovingDownRight || _player.isMovingUpRight)
            {
                // animate right
                _player.animationState.SetAnimationState("player_run_right");
            }
            else
            {
                DefaultAnimate();
            }

        }
        else if (_player.wasMovingLeft)
        {
            if (_player.isMovingLeft || _player.isMovingDownLeft || _player.isMovingUpLeft)
            {
                // animate left
                _player.animationState.SetAnimationState("player_run_left");
            }
            else
            {
                DefaultAnimate();
            }
        }
        else if (_player.wasMovingUp)
        {
            if (_player.isMovingUp || _player.isMovingUpRight || _player.isMovingUpLeft)
            {
                // animate up
                _player.animationState.SetAnimationState("player_run_up");
            }
            else
            {
                DefaultAnimate();
            }
        }
        else if (_player.wasMovingDown)
        {
            if (_player.isMovingDown || _player.isMovingDownRight || _player.isMovingDownLeft)
            {
                // animate up
                _player.animationState.SetAnimationState("player_run_down");
            }
            else
            {
                DefaultAnimate();
            }
        }
        else
        {
            DefaultAnimate();
        }

    }

    private void DefaultAnimate()
    {
        if (_player.moveVector.x > 0f)
        {
            _player.animationState.SetAnimationState("player_run_right");
        }
        else if (_player.moveVector.x < 0f)
        {
            _player.animationState.SetAnimationState("player_run_left");
        }
        else if (_player.moveVector.x == 0f && _player.moveVector.y > 0f)
        {
            _player.animationState.SetAnimationState("player_run_up");
        }
        else
        {
            _player.animationState.SetAnimationState("player_run_down");
        }
    }
}
