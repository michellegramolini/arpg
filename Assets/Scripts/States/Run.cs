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

        Animate();
    }

    private void MovePlayer()
    {
        _player.rb.velocity = _player.runSpeed * _player.moveVector;
    }

    private void Animate()
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