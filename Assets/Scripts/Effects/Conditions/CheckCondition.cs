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
    public class CheckConditions : EffectScript
    {   
        /// <summary>
        /// Valor de entrada a utilizar
        /// </summary>
        [SerializeReference,SubclassSelector]
        public IValue input;

        /// <summary>
        /// Lista de condiciones a comprobar
        /// </summary>
        [SerializeReference,SubclassSelector]
        public List<BaseCondition> conditions;

        public override IEnumerator execute(CardResolveOperator stack, Context context)
        {
            var value = input.getValueObj(context);
            var result = conditions.All( cond => cond.check(value, context));
            context.previousValues.Add(result);
            yield break;
        }
    }
}
