using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class Damaged : EnemyState
    {
        private EnemyController _enemy;

        public override void FixedUpdateState(EnemyController enemy)
        {

        }

        public override void LateUpdateState(EnemyController enemy)
        {

        }

        public override void StartState(EnemyController enemy)
        {
            this._enemy = enemy;

            DoDamage();
            StartCoroutine(DamageCoroutine());
        }

        public override void UpdateState(EnemyController enemy)
        {

        }

        private void DoDamage()
        {
            _enemy.health -= 1;
            Debug.Log($"Enemy Damaged! Health is {_enemy.health}");
        }

        private IEnumerator DamageCoroutine()
        {
            yield return new WaitForSeconds(0.2f);
            if (_enemy.health <= 0)
            {
                _enemy.SetState(_enemy.Dead);
            }
            else
            {
                _enemy.SetState(_enemy.Idle);
            }
        }
    }
}

