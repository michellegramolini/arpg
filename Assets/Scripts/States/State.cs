using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    public abstract void StartState(PlayerController player);

    public abstract void UpdateState(PlayerController player);

    public abstract void FixedUpdateState(PlayerController player);

    public abstract void LateUpdateState(PlayerController player);
}


