using System;
using System.Collections;
using CardHouse;
using CustomInspector;
using UnityEngine;

namespace Effect{
    /// <summary>
    /// Efecto que cura a un objetivo
    /// </summary>
    [Serializable]
    public class Heal : Targeted 
    {
        

        /// <summary>
        /// Cantidad de vida a curar
        /// </summary>
        [SerializeReference, SubclassSelector]
        public Value.Numeric amount;
 
        public override IEnumerator execute(CardResolveOperator stack, Effect.Context context)
        {
            var targets = targeter.getTargets(context);
            
            foreach(var target in targets){
                if(target is Entity entity){
                    entity.heal(amount.getValue(context));
                    
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
