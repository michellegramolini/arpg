using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class Idle : EnemyState
    {
        private EnemyController _enemy;

        public override void FixedUpdateState(EnemyController enemy)
        {

        }

        public override void LateUpdateState(EnemyController enemy)
        {
            this._enemy = enemy;
        }

        public override void StartState(EnemyController enemy)
        {

        }

        public override void UpdateState(EnemyController enemy)
        {

        }
    }
}

