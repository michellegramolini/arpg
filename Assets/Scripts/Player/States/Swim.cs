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
        GameObject splashEffect = ObjectPooler.Instance.SpawnFromPool("splash_effect", _player.transform.position, Quaternion.identity);
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
        if (_player.wasMovingRight)
        {
            if (_player.isMovingRight || _player.isMovingDownRight || _player.isMovingUpRight)
            {
                // animate right
                _player.animationState.SetAnimationState("player_swim_right");
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
                _player.animationState.SetAnimationState("player_swim_left");
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
                _player.animationState.SetAnimationState("player_swim_up");
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
                _player.animationState.SetAnimationState("player_swim_down");
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
            _player.animationState.SetAnimationState("player_swim_right");
        }
        else if (_player.moveVector.x < 0f)
        {
            _player.animationState.SetAnimationState("player_swim_left");
        }
        else if (_player.moveVector.x == 0f && _player.moveVector.y > 0f)
        {
            _player.animationState.SetAnimationState("player_swim_up");
        }
        else
        {
            _player.animationState.SetAnimationState("player_swim_down");
        }
    }
}
