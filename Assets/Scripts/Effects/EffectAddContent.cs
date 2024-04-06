using System;
using System.Collections;
using CardHouse;
using UnityEngine;

namespace Effect{
    /// <summary>
    /// Efecto que añade contenido a una habitación
    /// </summary>
    [Serializable]
    public class AddContent : Targeted
    {
        

        /// <summary>
        /// Cantidad a robar
        /// </summary>
        //TODO: allow use of variables
        public int amount = 2;

        public override IEnumerator execute(CardResolveOperator stack, TargettingContext context)
        {
            var targets = targeter.getTargets(context);
            Debug.Log($"Targets: {targets} {targets.Length}");
            foreach(var target in targets){
                Debug.Log($"Target: {target}");
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
                yield return CardTransferOperator.sendCardsFrom(source,amount, destination, 0.1f);
            }
        }
    }
}
