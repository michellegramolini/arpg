using System.Collections;
using System.Collections.Generic;
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

    }

    private void JumpPlayer()
    {
        if (_player.canWalk)
        {
            _player.rb.velocity = _player.walkSpeed * _player.moveVector;
        }
        // else stop movement?
        else
        {
            _player.rb.velocity = Vector2.zero;
        }


    }

    IEnumerator JumpSwitch()
    {
        _switch = true;
        yield return new WaitForSeconds(.2f);
        _switch = false;

    }
}
