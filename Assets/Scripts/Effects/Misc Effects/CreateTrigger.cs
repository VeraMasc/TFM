using System;
using System.Collections;
using System.Collections.Generic;
using CardHouse;
using Common.Coroutines;
using CustomInspector;
using Effect.Value;
using UnityEngine;

namespace Effect{
    /// <summary>
    /// Efecto que Crea un trigger separado
    /// </summary>
    [Serializable]
    public class CreateTrigger : EffectScript
    {
        /// <summary>
        /// Efectos del trigger a crear
        /// </summary>
        [SerializeReference,SubclassSelector]
        public List<EffectScript> trigger;


        public override IEnumerator execute(CardResolveOperator stack, Context context)
        {
            if(context.self is Card card){
                var routine =CardResolveOperator.singleton.triggerEffect(card,EffectChain.cloneFrom(trigger));
                yield return UCoroutine.Yield(routine);
            } 
            
            yield break;
        }


    }
}
