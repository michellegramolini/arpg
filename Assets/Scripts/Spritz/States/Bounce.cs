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

        }

        public override void LateUpdateState(SpritzController spritz)
        {
            _spritz.EnabeSprites();
            Animate();
        }

        public override void StartState(SpritzController spritz)
        {
            this._spritz = spritz;

            StopCoroutine(nameof(BounceCoroutine));
            StartCoroutine(nameof(BounceCoroutine));
        }

        public override void UpdateState(SpritzController spritz)
        {

        }

        private void ApplyMotionPhysics()
        {
            _spritz.moveVector = _moveVectors[random.Next(_moveVectors.Count)];
            //_spritz.DetectNextFacingTile(_spritz.moveVector, 1f);
            if (_spritz.previousState == _spritz.Spawn || !_spritz.DetectNextFacingTile(_spritz.moveVector, 1f))
            {
                //_spritz.moveVector = Vector2.zero;
                StopMotion();
            }
            else
            {
                _spritz.rb.velocity = _spritz.bounceSpeed * _spritz.moveVector;
            }
        }

        private void StopMotion()
        {
            _spritz.rb.velocity = Vector2.zero;
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
            ApplyMotionPhysics();
            yield return new WaitForSeconds(_spritz.animationState.GetClipLength("hop_up") * 0.8f);
            StopMotion();
            yield return new WaitForSeconds(_spritz.animationState.GetClipLength("hop_up") * 0.2f);
            _spritz.SetState(_spritz.Idle);
            yield break;
        }
    }
}


