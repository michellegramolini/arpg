using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spritz
{
    public class Dead : SpritzState
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

            Die();
        }

        public override void UpdateState(SpritzController spritz)
        {

        }

        private void Die()
        {
            _spritz.rb.velocity = Vector2.zero;
            Debug.Log($"{this} Enemy is Dead.");
            XPManager.Instance.AddExperience(_spritz.xpAmount);
            StartCoroutine(Respawn(1.2f));
            //Invoke(nameof(Respawn), 1.2f);
        }

        private IEnumerator Respawn(float seconds)
        {
            _spritz.sr.enabled = false;
            _spritz.bc.enabled = false;
            _spritz.shadowSprite.enabled = false;
            yield return new WaitForSeconds(seconds);
            // TODO:
            _spritz.health = 2;
            _spritz.SetState(_spritz.Idle);
            _spritz.gameObject.transform.position = _spritz.respawnPosition;
            _spritz.sr.enabled = true;
            _spritz.bc.enabled = true;
            _spritz.shadowSprite.enabled = true;
            //Debug.Log("did all");
        }

        //private void Respawn()
        //{
        //    _spritz.SetState(_spritz.Spawn);
        //}
    }
}

