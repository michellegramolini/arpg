using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spritz
{
    public abstract class SpritzState : MonoBehaviour
    {
        public abstract void StartState(SpritzController spritz);

        public abstract void UpdateState(SpritzController spritz);

        public abstract void FixedUpdateState(SpritzController spritz);

        public abstract void LateUpdateState(SpritzController spritz);
    }

}
