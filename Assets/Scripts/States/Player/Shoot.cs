using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : State
{
    private PlayerController player;

    public override void FixedUpdateState(PlayerController player)
    {
        throw new System.NotImplementedException();
    }

    public override void StartState(PlayerController player)
    {
        this.player = player;
    }

    public override void UpdateState(PlayerController player)
    {
        throw new System.NotImplementedException();
    }

    public override void LateUpdateState(PlayerController player)
    {

    }
}
