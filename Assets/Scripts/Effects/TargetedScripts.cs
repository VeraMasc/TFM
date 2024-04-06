using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Effect
{
    
    public class Targeted: EffectScript
    {
        /// <summary>
        /// Quién recibe el efecto
        /// </summary>
        [SerializeReference, SubclassSelector]
        public EffectTargeter targeter;
    }
}

