using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using Common.Coroutines;
using UnityEngine;

namespace Effect{

    /// <summary>
    /// Precalcula ciertos efectos cuando se pone la carta en el stack
    /// </summary>
    [Serializable]
    public class Precalculate: EffectScript, IPrecalculable
    {
        [SerializeReference, SubclassSelector]
        public List<IValueEffect> effects;

        /// <summary>
        /// No effect
        /// </summary>
        public override IEnumerator execute(CardResolveOperator stack, Effect.Context context){
            //Does nothing
            //TODO: handle targets having changed zones or becoming invalid
            var validators = effects.OfType<ValidateTargets>();
            foreach(var validator in validators){
                Debug.Log("Validating targets");
                validator.validate(context);
            }
            yield break;
        }

        /// <summary>
        /// Ejecuta los efectos precalculables
        /// </summary>
        /// <param name="stack"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public IEnumerator precalculate(CardResolveOperator stack, Effect.Context context){
            foreach(var effect in effects){
                yield return UCoroutine.Yield(effect.execute(stack,context));
            }
        }
        public static IEnumerator precalculateEffects(IEnumerable<EffectScript> effects, Context context){
        var stack = CardResolveOperator.singleton;
        foreach(var effect in effects){
            if(effect is IPrecalculable pre){
                yield return UCoroutine.Yield(pre.precalculate(stack, context));
            }
            else break;
        }
    }
    }
    
    

    /// <summary>
    /// Efectos que se consideran parte del precalculado
    /// </summary>
    public interface IPrecalculable{
        public IEnumerator execute(CardResolveOperator stack, Effect.Context context);
        public IEnumerator precalculate(CardResolveOperator stack, Effect.Context context);
    }

}
