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
            _spritz.DisableSprites();
        }

        public override void StartState(SpritzController spritz)
        {
            this._spritz = spritz;
            Die();
        }

        public override void UpdateState(SpritzController spritz)
        {
            Debug.Log("Should be dead dead dead");
        }

        private void Die()
        {
            _spritz.gameObject.SetActive(false);
            // play effect
            GameObject killEffect = ObjectPooler.Instance.SpawnFromPool("kill_effect", _spritz.characterHolder.transform.position, Quaternion.identity);
            XPManager.Instance.AddExperience(_spritz.xpAmount);
        }
    }
}

