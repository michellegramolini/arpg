using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spritz
{
    public class Spawn : SpritzState
    {
        private SpritzController _spritz;

        private bool _spawn;

        public override void FixedUpdateState(SpritzController spritz)
        {

        }

        public override void LateUpdateState(SpritzController spritz)
        {
            if (_spawn)
            {
                Animate();
            }
        }

        public override void StartState(SpritzController spritz)
        {
            this._spritz = spritz;

            _spawn = false;
            _spritz.sr.enabled = false;
            _spritz.shadowSprite.enabled = false;
            _spritz.bc.enabled = false;
            _spritz.shadowSprite.enabled = false;
        }

        public override void UpdateState(SpritzController spritz)
        {
            DetectPlayer();

            if (_spawn)
            {
                _spritz.health = 2;
                _spritz.gameObject.transform.position = _spritz.respawnPosition;
                _spritz.sr.enabled = true;
                _spritz.bc.enabled = true;

                //_spawn = false;
                Invoke(nameof(SpawnToIdle), _spritz.animationState.GetClipLength("spawn") + 0.1f);
            }
        }

        private void DetectPlayer()
        {
            Collider2D hitPlayer = Physics2D.OverlapCircle(transform.position, _spritz.playerDetectionRadius, _spritz.playerLayer);

            if (hitPlayer != null)
            {
                _spawn = true;
            }
        }

        private void SpawnToIdle()
        {
            _spawn = false;
            _spritz.SetState(_spritz.Idle);
        }

        private void Animate()
        {
            _spritz.animationState.SetAnimationState("spawn");
        }
    }
}

