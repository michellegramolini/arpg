using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Run : State
{
    private PlayerController player;

    public override void FixedUpdateState(PlayerController player)
    {
        throw new System.NotImplementedException();
    }

    public override void StartState(PlayerController player)
    {
        this.player = player;

        player.animationState.animator.speed *= 2f;
    }

    public override void UpdateState(PlayerController player)
    {

    }
}
