using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : State
{
    private PlayerController _player;

    public override void FixedUpdateState(PlayerController player)
    {
        StopMovement();
    }

    public override void StartState(PlayerController player)
    {
        this._player = player;
    }

    public override void UpdateState(PlayerController player)
    {
        if (_player.moveVector != _player.idleVector)
        {
            _player.SetState(_player.Walk);
        }
    }

    private void StopMovement()
    {
        _player.rb.velocity = Vector2.zero;
    }

}
