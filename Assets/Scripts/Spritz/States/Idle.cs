using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Spritz
{
    public class Idle : SpritzState
    {
        private SpritzController _spritz;

        public override void FixedUpdateState(SpritzController spritz)
        {
            // Stop movement while idle
            _spritz.rb.velocity = Vector2.zero;
        }

        public override void LateUpdateState(SpritzController spritz)
        {
            Animate();
        }

        public override void StartState(SpritzController spritz)
        {
            this._spritz = spritz;

            StartCoroutine(IdleCoroutine(UnityEngine.Random.Range(_spritz.idleTimeRange[0], _spritz.idleTimeRange[1])));
        }

        public override void UpdateState(SpritzController spritz)
        {

        }

        private void Animate()
        {
            if (_spritz.facingDirection == Vector2.up)
            {
                _spritz.animationState.SetAnimationState("idle_up");
            }
            else if (_spritz.facingDirection == Vector2.down)
            {
                _spritz.animationState.SetAnimationState("idle_down");
            }
            else if (_spritz.facingDirection == Vector2.left)
            {
                _spritz.animationState.SetAnimationState("idle_left");
            }
            else
            {
                _spritz.animationState.SetAnimationState("idle_right");
            }
        }

        private IEnumerator IdleCoroutine(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            _spritz.SetState(_spritz.Bounce);
        }

    }
}

