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
        if (_player.facingDirection == Vector2.right)
        {
            _player.animationState.SetAnimationState("player_walk_right");
        }
        else if (_player.facingDirection == Vector2.left)
        {
            _player.animationState.SetAnimationState("player_walk_left");
        }
        else if (_player.facingDirection == Vector2.up)
        {
            _player.animationState.SetAnimationState("player_walk_up");
        }
        else if (_player.facingDirection == Vector2.down)
        {
            _player.animationState.SetAnimationState("player_walk_down");
        }
        else if (_player.facingDirection.x > 0f && _player.facingDirection.y > 0)
        {
            _player.animationState.SetAnimationState("player_walk_up_right");
        }
        else if (_player.facingDirection.x < 0f && _player.facingDirection.y > 0)
        {
            _player.animationState.SetAnimationState("player_walk_up_left");
        }
        else if (_player.facingDirection.x > 0f && _player.facingDirection.y < 0)
        {
            _player.animationState.SetAnimationState("player_walk_down_right");
        }
        else
        {
            _player.animationState.SetAnimationState("player_walk_down_left");
        }

    }

}
