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
        // TODO:
    }

    public override void StartState(PlayerController player)
    {
        this._player = player;
    }

    public override void UpdateState(PlayerController player)
    {
        if (!_player.canSwim)
        {
            if (_player.canWalk)
            {
                _player.SetState(_player.Walk);
            }
        }
    }

    private void MovePlayer()
    {
        Debug.Log("i'm swimming!");

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
}
