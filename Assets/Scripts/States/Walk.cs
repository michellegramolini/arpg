using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    public override void UpdateState(PlayerController player)
    {
        if (_player.moveVector == _player.idleVector)
        {
            _player.SetState(_player.Idle);
        }

        Animate();

    }

    private void MovePlayer()
    {
        _player.rb.velocity = _player.walkSpeed * _player.moveVector;
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
