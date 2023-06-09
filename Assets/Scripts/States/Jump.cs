using System.Collections;
using UnityEngine;

public class Jump : State
{
    private PlayerController _player;

    bool _switch;
    float _jumpZ;

    public override void FixedUpdateState(PlayerController player)
    {
        if (_switch)
        {
            JumpPlayer();
        }
        else
        {
            _player.isJumping = false;
            _player.SetState(_player.Idle);
        }

    }

    public override void StartState(PlayerController player)
    {
        this._player = player;
        _jumpZ = 16f;

        _player.z += (int)_jumpZ;

        StartCoroutine(JumpSwitch());
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
        if (_player.canWalk)
        {
            _player.rb.velocity = _player.walkSpeed * _player.moveVector;
        }
        else
        {
            _player.rb.velocity = Vector2.zero;
        }
    }

    private void Animate()
    {
        if (_player.facingDirection.x > 0f)
        {
            _player.animationState.SetAnimationState("player_jump_right");
        }
        else if (_player.facingDirection.x < 0f)
        {
            _player.animationState.SetAnimationState("player_jump_left");
        }
        else if (_player.facingDirection.x == 0f && _player.facingDirection.y > 0f)
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
        yield return new WaitForSeconds(.2f);
        _switch = false;
    }
}
