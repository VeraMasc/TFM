using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        /// <summary>
        /// Sobreescribir este método para cambiar qué targets individuales se consideran válidos
        /// </summary>
        public virtual bool isValidTarget(ITargetable target,Effect.Context context){
            return true;
        }

        public override List<IManual> getManualInputs(){
            var nodes = targeter.getTargeterNodes();
            return nodes.OfType<IManual>().ToList();
        }

        /// <summary>
        /// Sobreescribir este método para cambiar qué conjunto global de targets se considera válido
        /// </summary>
        public virtual bool isValidResult(ITargetable[] targets,Effect.Context context){
            return true;
        }

        public override IEnumerator execute(CardResolveOperator stack, Context context)
        {
            var targets = targeter.getTargets(context)
                .Where(t => isValidTarget(t,context));
            
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

