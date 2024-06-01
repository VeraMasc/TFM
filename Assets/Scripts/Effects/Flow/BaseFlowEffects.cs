using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Common.Coroutines;
using UnityEngine;

namespace Effect
{
    /// <summary>
    /// Comprueba una serie de condiciones y devuelve si se cumplen todas
    /// </summary>
    [Serializable]
    public class ExecuteIf : ConditionalEffect
    {   

        [SerializeReference,SubclassSelector]
        public List<EffectScript> effects;

        public override IEnumerator onSuccess(Context context)
        {
            var chain = EffectChain.cloneFrom(effects);
            
            foreach(var effect in chain.list){
                yield return UCoroutine.Yield(effect.execute(CardResolveOperator.singleton,context));
            }
        }
    }
}