using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Walk : State
{
    private PlayerController _player;
    private bool _canWalk;
    private string _tileKey;

    public override void FixedUpdateState(PlayerController player)
    {
        EnableWalk();
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

        if (player.isShifting)
        {
            _player.SetState(_player.Run);
        }

    }

    public override void LateUpdateState(PlayerController player)
    {
        Animate();
    }

    private void MovePlayer()
    {
        if (_canWalk)
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

    private void EnableWalk()
    {

        // Get tile key from facing direction...
        _tileKey = _player.GetTileKeyFromFacingDirection();

        if (_player.tileDetector.GetTileProp(_tileKey, "height_value") != null)
        {
            int? height = Convert.ToInt32(_player.tileDetector.GetTileProp(_tileKey, "height_value").m_Value);
            // Get tile height from tile key
            if (height != null)
            {
                if (height <= _player.z)
                {
                    _canWalk = true;
                }
                else
                {
                    _canWalk = false;
                }
            }
            else
            {
                _canWalk = true;
            }
        }
        else
        {
            _canWalk = true;
        }
    }

}
