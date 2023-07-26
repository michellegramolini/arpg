using System.Collections;
using UnityEngine;

public class Jump : State
{
    private PlayerController _player;

    bool _switch;
    float _jumpZ;

    //private Vector2 _lockedFacingDirection;


    public override void FixedUpdateState(PlayerController player)
    {
        if (_switch)
        {
            JumpPlayer();
        }
        else
        {
            _player.isJumping = false;
            if (_player.canSwim)
            {
                _player.SetState(_player.Swim);
            }
            else
            {
                _player.SetState(_player.Fall);
            }

        }
    }

    public override void StartState(PlayerController player)
    {
        this._player = player;
        _jumpZ = 16f;

        _player.z += (int)_jumpZ;

        //_lockedFacingDirection = _player.facingDirection;
        _player.SetLockedMoveVectors();

        StopCoroutine(nameof(JumpSwitch));
        StartCoroutine(nameof(JumpSwitch));
    }

    public override void UpdateState(PlayerController player)
    {

    }

    public override void LateUpdateState(PlayerController player)
    {
        Animate();
    }

    private void JumpPlayer()
    {
        if (_player.canMove)
        {
            _player.rb.velocity = _player.jumpSpeed * _player.moveVector;
        }
        else
        {
            _player.rb.velocity = Vector2.zero;
        }
    }

    private void Animate()
    {
        if (_player.locked_moveVector == _player.idleVector)
        {
            DefaultAnimate(_player.locked_facingDirection);
        }
        else
        {
            if (_player.locked_wasMovingRight)
            {
                if (_player.locked_isMovingRight || _player.locked_isMovingDownRight || _player.locked_isMovingUpRight)
                {
                    // animate right
                    _player.animationState.SetAnimationState("player_jump_right");
                }
                else
                {
                    DefaultAnimate(_player.locked_moveVector);
                }

            }
            else if (_player.locked_wasMovingLeft)
            {
                if (_player.locked_isMovingLeft || _player.locked_isMovingDownLeft || _player.locked_isMovingUpLeft)
                {
                    // animate left
                    _player.animationState.SetAnimationState("player_jump_left");
                }
                else
                {
                    DefaultAnimate(_player.locked_moveVector);
                }
            }
            else if (_player.locked_wasMovingUp)
            {
                if (_player.locked_isMovingUp || _player.locked_isMovingUpRight || _player.locked_isMovingUpLeft)
                {
                    // animate up
                    _player.animationState.SetAnimationState("player_jump_up");
                }
                else
                {
                    DefaultAnimate(_player.locked_moveVector);
                }
            }
            else if (_player.locked_wasMovingDown)
            {
                if (_player.locked_isMovingDown || _player.locked_isMovingDownRight || _player.locked_isMovingDownLeft)
                {
                    // animate up
                    _player.animationState.SetAnimationState("player_jump_down");
                }
                else
                {
                    DefaultAnimate(_player.locked_moveVector);
                }
            }
            else
            {
                DefaultAnimate(_player.locked_moveVector);
            }
        }
    }

    private void DefaultAnimate(Vector2 vector)
    {
        if (vector.x > 0f)
        {
            _player.animationState.SetAnimationState("player_jump_right");
        }
        else if (vector.x < 0f)
        {
            _player.animationState.SetAnimationState("player_jump_left");
        }
        else if (vector.x == 0f && vector.y > 0f)
        {
            _player.animationState.SetAnimationState("player_jump_up");
        }
        else
        {
            _player.animationState.SetAnimationState("player_jump_down");
        }
    }

    IEnumerator JumpSwitch()
    {
        _switch = true;
        yield return new WaitForSeconds(_player.animationState.GetClipLength("player_jump_right") + .1f);
        _switch = false;

        yield break;
    }
}
