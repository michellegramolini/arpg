using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall : State
{
    private PlayerController _player;

    private Vector2 _fallVector = new(0f, -1f);

    public override void FixedUpdateState(PlayerController player)
    {
        MovePlayer();
    }

    public override void LateUpdateState(PlayerController player)
    {

    }

    public override void StartState(PlayerController player)
    {
        this._player = player;
    }

    public override void UpdateState(PlayerController player)
    {
        if (!_player.onWall)
        {
            _player.SetState(_player.Idle);
        }
    }

    private void MovePlayer()
    {
        _player.rb.velocity = _fallVector * _player.fallSpeed;
    }
}
