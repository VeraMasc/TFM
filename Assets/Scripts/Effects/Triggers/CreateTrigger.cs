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
                var routine =CardResolveOperator.singleton.triggerEffect(card,EffectChain.cloneFrom(trigger),false);
                yield return UCoroutine.Yield(routine);
            } 
            
            yield break;
        }


    }

    /// <summary>
    /// Efecto que Crea un trigger separado que se activa con retraso
    /// </summary>
    [Serializable]
    public class CreateDelayedTrigger : EffectScript
    {
        /// <summary>
        /// Efectos y condiciones del trigger a crear
        /// </summary>
        [SerializeReference,SubclassSelector]
        public Ability delayed;


        public override IEnumerator execute(CardResolveOperator stack, Context context)
        {
            //TODO: Delayed Trigger
            throw new NotImplementedException();
            // if(context.self is Card card){
            //     var routine =CardResolveOperator.singleton.triggerEffect(card,EffectChain.cloneFrom(trigger));
            //     yield return UCoroutine.Yield(routine);
            // } 
            
            yield break;
        }


    }
}
