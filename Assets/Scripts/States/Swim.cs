using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swim : State
{
    private PlayerController _player;

    public override void FixedUpdateState(PlayerController player)
    {
        MovePlayer();
    }

    public override void LateUpdateState(PlayerController player)
    {
        Animate();
    }

    public override void StartState(PlayerController player)
    {
        this._player = player;
    }

    public override void UpdateState(PlayerController player)
    {
        if (!_player.canSwim)
        {
            if (_player.canMove)
            {
                _player.SetState(_player.Walk);
            }
        }
    }

    private void MovePlayer()
    {
        if (_player.canSwim)
        {
            _player.rb.velocity = _player.swimSpeed * _player.moveVector;
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
            _player.animationState.SetAnimationState("kaia_swim_right");
        }
        else if (_player.moveVector.x < 0f)
        {
            _player.animationState.SetAnimationState("kaia_swim_left");
        }
        else if (_player.moveVector.x == 0f && _player.moveVector.y > 0f)
        {
            _player.animationState.SetAnimationState("kaia_swim_up");
        }
        else
        {
            _player.animationState.SetAnimationState("kaia_swim_down");
        }
    }
}
