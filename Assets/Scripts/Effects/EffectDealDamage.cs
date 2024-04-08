using System;
using System.Collections;
using CardHouse;
using CustomInspector;
using UnityEngine;

namespace Effect{
    /// <summary>
    /// Efecto que hace daño a un objetivo
    /// </summary>
    [Serializable]
    public class DealDamage : Targeted 
    {
        

        /// <summary>
        /// Cantidad de daño a realizar
        /// </summary>
        //TODO: allow use of variables
        [SerializeReference, SubclassSelector]
        public Value.Numeric amount;

        public override IEnumerator execute(CardResolveOperator stack, Context context)
        {
            var targets = targeter.getTargets(context);
            
            foreach(var target in targets){
                if(target is Entity entity){
                    entity.health -= amount.getValue();
                    //TODO: Use event to deal the damage
                    //TODO: Use damage animation corroutine
                    //TODO: Allow simultaneous damage
                    yield return new WaitForSeconds(0.2f);
                }
                else {
                    Debug.Log($"Can't deal damage to non-entity of type {target?.GetType()}", (UnityEngine.Object)target);
                }
            }
        }
    }
}
