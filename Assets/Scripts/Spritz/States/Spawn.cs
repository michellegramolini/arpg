using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spritz
{
    public class Spawn : SpritzState
    {
        private SpritzController _spritz;
        private Coroutine _spawnCoroutine;

        public override void FixedUpdateState(SpritzController spritz)
        {

        }

        public override void LateUpdateState(SpritzController spritz)
        {

        }

        public override void StartState(SpritzController spritz)
        {
            this._spritz = spritz;
        }

        public override void UpdateState(SpritzController spritz)
        {
            if (_spritz.playerDetected)
            {
                _spritz.EnabeSprites();
                Animate();
                if (_spawnCoroutine == null)
                {
                    _spawnCoroutine = StartCoroutine(nameof(SpawnCoroutine));
                }
            }
            else
            {
                _spritz.DisableSprites();
            }
        }

        private IEnumerator SpawnCoroutine()
        {
            yield return new WaitForSeconds(_spritz.animationState.GetClipLength("spawn") + 0.1f);
            _spritz.SetState(_spritz.Bounce);
            yield break;
        }

        private void Animate()
        {
            _spritz.animationState.SetAnimationState("spawn");
        }
    }
}

