using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Common.Coroutines;
using UnityEngine;


namespace Effect
{
    /// <summary>
    /// Clase madre de los efectos opcionales
    /// </summary>
    public abstract class ConditionalEffect : EffectScript
    {   
        /// <summary>
        /// Valor de entrada a utilizar
        /// </summary>
        [SerializeReference,SubclassSelector]
        public IValue input  ;

        /// <summary>
        /// Lista de condiciones a comprobar
        /// </summary>
        [SerializeReference,SubclassSelector]
        public List<BaseCondition> conditions = new();

        public override IEnumerator execute(CardResolveOperator stack, Context context)
        {
            var value = input.getValueObj(context);
            bool result;
            if(conditions.Any()){
                result = conditions.All( cond => cond.check(value, context));
            }else{
                result = (bool)value; //Usar el valor de input si no hay condiciones
            }
            
            if(result){
                yield return stack.StartCoroutine(onSuccess(context));
            }else{
                yield return stack.StartCoroutine(onFailure(context));
            }
            yield break;
        }

        public virtual IEnumerator onSuccess(Context context){
            yield break;
        }

        public virtual IEnumerator onFailure(Context context){
            yield break;
        }
    }


    /// <summary>
    /// Comprueba una serie de condiciones y devuelve si se cumplen todas
    /// </summary>
    [Serializable]
    public class CheckConditions : ConditionalEffect
    {   
        public override IEnumerator onSuccess(Context context){
            context.previousValues.Add(true);
            yield break;
        }

        public override IEnumerator onFailure(Context context){
            context.previousValues.Add(false);
            yield break;
        }
    }
}
