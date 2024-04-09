using System;
using System.Collections;
using System.Collections.Generic;
using Common.Coroutines;
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

        public override IEnumerator execute(CardResolveOperator stack, Context context)
        {
            var targets = targeter.getTargets(context);
            
            foreach(var target in targets){
                yield return UCoroutine.Yield(executeForeach(target,stack,context));
            }
        }

        /// <summary>
        /// Ejecuta el mismo código para cada target
        /// </summary>
        public virtual IEnumerator executeForeach(ITargetable target,CardResolveOperator stack, Context context)
        {
            throw new NotImplementedException();
        }

    }
}

