using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spritz
{
    public class Damaged : SpritzState
    {
        private SpritzController _spritz;

        public override void FixedUpdateState(SpritzController spritz)
        {

        }

        public override void LateUpdateState(SpritzController spritz)
        {

        }

        public override void StartState(SpritzController spritz)
        {
            this._spritz = spritz;

            DoDamage();
            StartCoroutine(DamageCoroutine());
        }

        public override void UpdateState(SpritzController spritz)
        {

        }

        private void DoDamage()
        {
            // TODO: configure amounts
            _spritz.health -= 1;
            DamageManager.Instance.AddDamage(1);
            // TODO: popups
            DamageManager.Instance.GenerateDamagePopup(transform.position, 1);
            //Debug.Log($"Enemy Damaged! Health is {_enemy.health}");
        }

        private IEnumerator DamageCoroutine()
        {
            // TODO: should probably be in fixedupdate
            // TODO: if knockback over edge
            _spritz.rb.velocity = _spritz.hitDirection * _spritz.knockbackForce;
            _spritz.rb.drag += 2f;
            yield return new WaitForSeconds(0.2f);
            _spritz.rb.drag = 0f;
            //_spritz.rb.velocity = Vector2.zero;

            if (_spritz.health <= 0)
            {
                _spritz.SetState(_spritz.Dead);
            }
            else
            {
                _spritz.SetState(_spritz.Idle);
            }
        }
    }
}

