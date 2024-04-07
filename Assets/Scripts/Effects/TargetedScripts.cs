using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Effect
{
    [Serializable]
    public class Targeted: EffectScript
    {
        /// <summary>
        /// Quién recibe el efecto
        /// </summary>
        [SerializeReference, SubclassSelector]
        public EffectTargeter targeter;

    }
}

