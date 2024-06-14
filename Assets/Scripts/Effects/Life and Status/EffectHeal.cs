using System;
using System.Collections;
using CardHouse;
using Common.Coroutines;
using CustomInspector;
using Effect.Value;
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

        public bool overheal=true;
 
        public override IEnumerator execute(CardResolveOperator stack, Effect.Context context)
        {
            var targets = targeter.getTargets(context);
            
            foreach(var target in targets){
                if(target is Entity entity){
                    yield return entity.heal(amount.getValue(context), overheal)
                        .Start(entity);
                    
                    //TODO: Allow simultaneous damage
                    yield return new WaitForSeconds(0.2f);
                }
                else {
                    Debug.Log($"Can't deal damage to non-entity of type {target?.GetType()}", (UnityEngine.Object)target);
                }
            }
        }
    }

     /// <summary>
    /// Efecto que cura a un objetivo
    /// </summary>
    [Serializable]
    public class Revive : Targeted 
    {
        


 
        public override IEnumerator execute(CardResolveOperator stack, Effect.Context context)
        {
            var targets = targeter.getTargets(context);
            
            foreach(var target in targets){
                if(target is Entity entity){
                    yield return entity.tryRevive().Start(entity);
                    
                }
                else {
                    Debug.Log($"Can't deal damage to non-entity of type {target?.GetType()}", (UnityEngine.Object)target);
                }
            }
        }
    }
}
