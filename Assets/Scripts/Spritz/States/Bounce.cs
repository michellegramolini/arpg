using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spritz
{
    public class Bounce : SpritzState
    {
        private SpritzController _spritz;

        private System.Random random = new System.Random();

        private bool _bounce;

        // randomize move vector
        private readonly List<Vector2> _moveVectors = new List<Vector2>{
            Vector2.up,
            Vector2.down,
            Vector2.left,
            Vector2.right
        };
        //

        public override void FixedUpdateState(SpritzController spritz)
        {
            if (_bounce)
            {
                BounceAround();
            }
        }

        public override void LateUpdateState(SpritzController spritz)
        {
            Animate();
        }

        public override void StartState(SpritzController spritz)
        {
            this._spritz = spritz;
            if (!_spritz.shadowSprite.enabled)
            {
                _spritz.shadowSprite.enabled = true;
            }
            StartCoroutine(BounceCoroutine());
        }

        public override void UpdateState(SpritzController spritz)
        {
            if (_spritz.currentState == _spritz.Dead)
            {
                _bounce = false;
            }
        }

        private void BounceAround()
        {
            if (_spritz.canMove)
            {
                _spritz.rb.velocity = _spritz.bounceSpeed * _spritz.moveVector;
            }
            else
            {
                _spritz.rb.velocity = Vector2.zero;
            }
        }


        private void Animate()
        {
            if (_spritz.facingDirection == Vector2.up)
            {
                _spritz.animationState.SetAnimationState("hop_up");
            }
            else if (_spritz.facingDirection == Vector2.down)
            {
                _spritz.animationState.SetAnimationState("hop_down");
            }
            else if (_spritz.facingDirection == Vector2.left)
            {
                _spritz.animationState.SetAnimationState("hop_left");
            }
            else
            {
                _spritz.animationState.SetAnimationState("hop_right");
            }
        }

        IEnumerator BounceCoroutine()
        {
            //while (_spritz.currentState != _spritz.Dead)
            //{
            _bounce = true;
            _spritz.moveVector = _moveVectors[random.Next(_moveVectors.Count)];
            yield return new WaitForSeconds(_spritz.animationState.GetClipLength("hop_up"));
            _spritz.SetState(_spritz.Idle);
            //}
        }
    }
}


