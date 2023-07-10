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
            //Debug.Log($"{this} Enemy is Dead.");
            XPManager.Instance.AddExperience(_enemy.xpAmount);
            //_enemy.gameObject.SetActive(false);
            StartCoroutine(Respawn(1.2f));
        }

        private IEnumerator Respawn(float seconds)
        {
            _enemy.sr.enabled = false;
            _enemy.bc.enabled = false;
            yield return new WaitForSeconds(seconds);
            // TODO:
            _enemy.health = 2;
            _enemy.SetState(_enemy.Idle);
            _enemy.gameObject.transform.position = _enemy.respawnPosition;
            _enemy.sr.enabled = true;
            _enemy.bc.enabled = true;
        }
    }
}

