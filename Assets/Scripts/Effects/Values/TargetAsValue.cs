using UnityEngine;

using System;
using Effect.Value;
using System.Collections;

namespace Effect{
    /// <summary>
    /// Guarda un target como valor
    /// </summary>
    [Serializable]
    public class TargetAsValue:Targeted, IValueEffect
    {
        public override IEnumerator execute(CardResolveOperator stack, Context context)
        {
            var targets = targeter.getTargets(context);
            context.previousValues.Add(targets);
            yield break;
        }
    }
    

}