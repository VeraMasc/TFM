using System;
using System.Collections;
using System.Collections.Generic;
using CardHouse;
using CustomInspector;
using Effect.Value;
using UnityEngine;
using System.Linq;
using Common.Coroutines;

namespace Effect{
    /// <summary>
    /// Activa habilidades que tienen el ID especificado
    /// </summary>
    [Serializable]
    public class ActivateTriggerByID : Targeted, IValueEffect
    {
        [SerializeReference,SubclassSelector]
        public IValue ID = new TextString();

        /// <summary>
        /// Triggerea solo las habilidades que están activas
        /// </summary>
        public bool activeOnly;

        public override IEnumerator executeForeach(ITargetable target, CardResolveOperator stack, Context context)
        {
            if(target is Card card && card.data is MyCardSetup setup){
                var targetID = (string)ID.getValueObj(context);
                Debug.Log($"Activating ${targetID} for {card}" ,card);
                
                var abilities = setup.effects.getAllAbilities()
                    .Where(ab => ab.id == targetID && (!activeOnly || ab.isCurrentlyActive));
                foreach(var ability in abilities){
                    yield return ability.executeAbility(context)
                        .Start(ability.source?? (MonoBehaviour)stack);
                }
            }
            else{
                Debug.LogError($"Can't trigger abilities in targets of type {target?.GetType()}", (UnityEngine.Object)target);
            }
        }

    }

 
}
