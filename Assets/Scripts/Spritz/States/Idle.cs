using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Spritz
{
    public class Idle : SpritzState
    {
        private SpritzController _spritz;

        private System.Random random = new System.Random();


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

        }

        public override void LateUpdateState(SpritzController spritz)
        {
            Animate();
        }

        public override void StartState(SpritzController spritz)
        {
            this._spritz = spritz;
            //_spritz.moveVector = Vector2.zero;
            StartCoroutine(BounceAround());
        }

        public override void UpdateState(SpritzController spritz)
        {

        }

        private void Bounce()
        {
            _spritz.rb.velocity = _spritz.bounceSpeed * _spritz.moveVector;
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

        IEnumerator BounceAround()
        {
            while (_spritz.currentState == _spritz.Idle)
            {
                _spritz.moveVector = _moveVectors[random.Next(_moveVectors.Count)];
                Bounce();
                yield return new WaitForSeconds(_spritz.animationState.GetClipLength("hop_up"));
            }
        }
    }
}

