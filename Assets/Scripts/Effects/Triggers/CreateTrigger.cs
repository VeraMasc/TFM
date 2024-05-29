using System;
using System.Collections;
using System.Collections.Generic;
using CardHouse;
using Common.Coroutines;
using CustomInspector;
using Effect.Duration;
using Effect.Value;
using UnityEngine;
using UnityEngine.Events;

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
    public class CreateDelayedTrigger : CreateTrigger
    {
        /// <summary>
        /// Duración hasta que el trigger se produce
        /// </summary>
        [SerializeReference,SubclassSelector]
        public IDuration delay;


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

    namespace Duration
    {
        /// <summary>
        /// Encapsula las clases que representan duraciones
        /// </summary>
        public interface IDuration{
            public void setContext(Context context);
            public void subscribe(Action action);

            public void unsubscribe(Action action);

        }

        /// <summary>
        /// Dura hasta que se produce un trigger
        /// </summary>
        [Serializable]
        public class UntilTrigger:IDuration{
            public BaseTrigger<object> trigger;

            public UnityAction listener;

            /// <summary>
            /// Condiciones que ha de cumplir
            /// </summary>
            [SerializeReference,SubclassSelector]
            public BaseCondition condition;

            public Context context;
            public void setContext(Context context){
                this.context = context;
                listener+=checkIfFinished;
                trigger.events.AddListener(listener);
            }

            public bool isFinished;

            public void checkIfFinished(){
                isFinished = condition.check(null,context);
            }

            public IEnumerator waitTillFinished => UCoroutine.YieldAwait(()=>isFinished);

            public void subscribe(Action action){
                
            }

            public void unsubscribe(Action action){

            }
        }
    }
    
}
