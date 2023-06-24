using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class Dead : EnemyState
    {
        private EnemyController _enemy;

        public override void FixedUpdateState(EnemyController enemy)
        {
            throw new System.NotImplementedException();
        }

        public override void LateUpdateState(EnemyController enemy)
        {
            throw new System.NotImplementedException();
        }

        public override void StartState(EnemyController enemy)
        {
            this._enemy = enemy;

            Die();
        }

        public override void UpdateState(EnemyController enemy)
        {
            throw new System.NotImplementedException();
        }

        private void Die()
        {
            Debug.Log($"{this} Enemy is Dead.");
            XPManager.Instance.AddExperience(_enemy.xpAmount);
            _enemy.gameObject.SetActive(false);
        }
    }
}

