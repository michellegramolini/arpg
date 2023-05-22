using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : State
{
    private PlayerController _player;

    bool _switch;

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

        StartCoroutine(JumpSwitch());
    }

    public override void UpdateState(PlayerController player)
    {

    }

    private void JumpPlayer()
    {
        // ?
        Debug.Log("Jumpin!");
    }

    IEnumerator JumpSwitch()
    {
        _switch = true;
        yield return new WaitForSeconds(.2f);
        _switch = false;

    }
}
