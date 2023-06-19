using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Walk : State
{
    private PlayerController _player;
    //private bool _canMove;
    //private string _tileKey;

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

        if (player.isShifting)
        {
            _player.SetState(_player.Run);
        }
        else if (_player.isJumping)
        {
            _player.SetState(_player.Jump);
        }
        else if (_player.canSwim)
        {
            _player.SetState(_player.Swim);
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

    //private void MovePlayer()
    //{
    //    _player.rb.velocity = _player.walkSpeed * _player.moveVector;
    //}

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

    //private void EnableWalk()
    //{
    //    // Get tile key from facing direction...
    //    _tileKey = _player.GetTileKeyFromFacingDirection();

    //    if (_player.tileDetector.GetTileProp(_tileKey, "height_value") != null)
    //    {
    //        int? height = Convert.ToInt32(_player.tileDetector.GetTileProp(_tileKey, "height_value").m_Value);
    //        // Get tile height from tile key
    //        if (height != null)
    //        {
    //            if (height <= _player.z)
    //            {
    //                _canMove = true;
    //            }
    //            else
    //            {
    //                _canMove = false;
    //            }
    //        }
    //        else
    //        {
    //            _canMove = true;
    //        }
    //    }
    //    else
    //    {
    //        _canMove = true;
    //    }
    //}

}
