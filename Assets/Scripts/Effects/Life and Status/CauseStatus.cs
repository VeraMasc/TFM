using System.Collections;
using System.Collections.Generic;
using Effect;
using Effect.Status;
using UnityEngine;


namespace Effect
{

    

    /// <summary>
    /// Causa un efecto de estatus en una entidad
    /// </summary>
    public class CauseStatus : Targeted
    {
        /// <summary>
        /// Status a aplicar //TODO: implementar con Effect Values
        /// </summary>
        public BaseStatus status;
        public override IEnumerator executeForeach(ITargetable target, CardResolveOperator stack, Context context)
        {
            if (target is Entity entity){
                yield return entity.applyStatus(status);
            }
        }
    }
}