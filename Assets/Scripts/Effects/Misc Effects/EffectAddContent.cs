using System;
using System.Collections;
using CardHouse;
using CustomInspector;
using UnityEngine;

namespace Effect{
    /// <summary>
    /// Efecto que añade contenido a una habitación
    /// </summary>
    [Serializable]
    public class AddContent : Targeted
    {
        

        /// <summary>
        /// Cantidad a ce contenido a añadir
        /// </summary>
        [SerializeReference, SubclassSelector]
        public Value.Numeric amount;

        public override IEnumerator execute(CardResolveOperator stack, Effect.Context context)
        {
            var targets = targeter.getTargets(context);
            
            foreach(var target in targets){
                CardGroup destination = null;

                if(target is Card card){ //If card use attach group
                    destination= card.attachedGroup;
                }
                else if (target is CardGroup group){ //If group use the group
                    destination= group;
                }
                var source = ExplorationController.singleton.content;
               
                if(destination==null){
                    Debug.LogError("No content group to add to", (UnityEngine.Object) target);
                    continue;
                }
                yield return CardTransferOperator.sendCardsFrom(source, amount.getValue(), destination, 0.1f);
            }
        }

        /// <summary>
        /// Sobreescribir este método para cambiar qué targets individuales se consideran válidos
        /// </summary>
        public override bool isValidTarget(ITargetable target,Effect.Context context){
            if(target is Card card && card?.data is RoomCard room){
                //Accept only rooms as targets
                return targeter.isValidTarget(target, context);
            }
            return false;
        }

    
    }
}
