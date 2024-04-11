using System;
using System.Collections;
using UnityEngine;


namespace Effect
{
    [Serializable]
    public class Draw : Targeted
    {
        /// <summary>
        /// Cantidad a robar
        /// </summary>
        [SerializeReference, SubclassSelector]
        public Value.Numeric amount;

        public override IEnumerator executeForeach(ITargetable target,CardResolveOperator stack, Context context)
        {
            if(target is Entity entity){
                yield return entity.draw(amount.getValue(context));
                
            }else{
                Debug.LogError($"Target of {this.GetType().Name} is \"{target.GetType().Name}\" not an entity", (UnityEngine.Object)context.self);
            }
        }
    }
}
