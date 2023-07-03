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
            _spritz.animationState.SetAnimationState("test");
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
            yield return new WaitForSeconds(0.2f);
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

