using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : State
{
    private PlayerController _player;

    bool _switch;

    public override void FixedUpdateState(PlayerController player)
    {

    }

    public override void StartState(PlayerController player)
    {
        this._player = player;

        StartCoroutine(JumpSwitch());
    }

    public override void UpdateState(PlayerController player)
    {
        if (!_switch)
        {
            _player.isJumping = false;
            _player.SetState(_player.Idle);
        }
    }

    public override void LateUpdateState(PlayerController player)
    {
        Animate();
    }

    //private void JumpPlayer()
    //{
    //    // ?
    //    Debug.Log("jumping switch");
    //}

    IEnumerator JumpSwitch()
    {
        _switch = true;
        yield return new WaitForSeconds(_player.animationState.GetClipLength("player_jump_right") + 0.1f);
        _switch = false;

    }

    private void Animate()
    {
        // use facing direction and set facing animation
        if (_player.facingDirection == Vector2.up)
        {
            _player.animationState.SetAnimationState("player_jump_up");
        }
        else if (_player.facingDirection == Vector2.down)
        {
            _player.animationState.SetAnimationState("player_jump_down");
        }
        else if (_player.facingDirection.x > 0f)
        {
            _player.animationState.SetAnimationState("player_jump_right");
        }
        else
        {
            _player.animationState.SetAnimationState("player_jump_left");
        }
    }
}
