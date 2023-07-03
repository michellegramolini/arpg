using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyState : MonoBehaviour
{
    public abstract void StartState(EnemyController enemy);

    public abstract void UpdateState(EnemyController enemy);

    public abstract void FixedUpdateState(EnemyController enemy);

    public abstract void LateUpdateState(EnemyController enemy);
}
