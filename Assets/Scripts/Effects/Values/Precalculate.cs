using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Effect{

    /// <summary>
    /// Precalcula ciertos efectos cuando se pone la carta en el stack
    /// </summary>
    [Serializable]
    public class Precalculate: EffectScript,IPrecalculable
    {
        [SerializeReference, SubclassSelector]
        List<IValueEffect> effects;

        /// <summary>
        /// Executes the effect
        /// </summary>
        public override IEnumerator execute(CardResolveOperator stack, Effect.Context context){
            throw new NotImplementedException();
            
        }
    }

    /// <summary>
    /// Engloba todos los efectos que necesitan ser precalculados
    /// </summary>
    public interface IPrecalculable{

    }
}
