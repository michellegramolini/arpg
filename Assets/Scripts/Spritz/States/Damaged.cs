using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spritz
{
    public class Damaged : SpritzState
    {
        private SpritzController _spritz;
        private float _originalDrag;

        public override void FixedUpdateState(SpritzController spritz)
        {
            if (_spritz.IsStandingOnBadTile())
            {
                _spritz.rb.velocity = Vector2.zero;
                _spritz.rb.velocity = -1.5f * _spritz.hitDirection;
            }
        }

        public override void LateUpdateState(SpritzController spritz)
        {
            // TODO: damage animation
        }

        public override void StartState(SpritzController spritz)
        {
            this._spritz = spritz;
            _originalDrag = _spritz.rb.drag;

            StopCoroutine(nameof(DamageCoroutine));
            StartCoroutine(nameof(DamageCoroutine));
        }

        public override void UpdateState(SpritzController spritz)
        {

        }

        private void DoDamage()
        {
            // TODO: configure amounts
            _spritz.health -= 1;
            DamageManager.Instance.AddDamage(1);

        }

        private void DoPopups()
        {
            DamageManager.Instance.GenerateDamagePopup("damage_popup", transform.position, 1);
        }

        private void ResetPhysics()
        {
            _spritz.rb.drag = _originalDrag;
            _spritz.rb.velocity = Vector2.zero;
        }

        private void ApplyKnockbackForce()
        {
            _spritz.rb.drag += _spritz.knockbackDrag;
            _spritz.rb.velocity = _spritz.hitDirection * _spritz.knockbackForce;
        }

        private IEnumerator DamageCoroutine()
        {
            // temporarily disabling the box collider should prevent accidental double hits. 
            _spritz.bc.enabled = false;
            ResetPhysics();

            DoDamage();
            DoPopups();
            // HACK:
            ApplyKnockbackForce();
            // this is temporary, but just a way of trying to get some springyness into the knockback
            yield return new WaitForSeconds(0.1f);
            ResetPhysics();
            _spritz.rb.velocity = (-_spritz.knockbackForce * 0.5f) * _spritz.hitDirection;
            yield return new WaitForSeconds(0.08f);
            ResetPhysics();
            _spritz.bc.enabled = true;
            if (_spritz.health <= 0)
            {
                _spritz.SetState(_spritz.Dead);
            }
            else
            {
                _spritz.SetState(_spritz.Idle);
            }

            yield break;

        }

    }
}

