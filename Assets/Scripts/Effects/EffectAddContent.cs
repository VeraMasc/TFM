using System;
using System.Collections;
using CardHouse;
using UnityEngine;

namespace Effect{
    /// <summary>
    /// Efecto que añade contenido a una habitación
    /// </summary>
    [Serializable]
    public class AddContent : EffectScript
    {
        /// <summary>
        /// Quién roba las cartas
        /// </summary>
        [SerializeReference, SubclassSelector]
        public EffectTargeter targeter;

        /// <summary>
        /// Cantidad a robar
        /// </summary>
        //TODO: allow use of variables
        public int amount = 2;

        public override IEnumerator execute(CardResolveOperator stack, TargettingContext context)
        {
            var targets = targeter.getTargets(context);
            foreach(var target in targets){
                var source = ExplorationController.singleton.content;
                var destination = target.GetComponent<CardGroup>();
                if(destination==null){
                    Debug.LogError("No content group to add to", (UnityEngine.Object) target.GetComponent<ITargetable>());
                    continue;
                }
                yield return CardTransferOperator.sendCardsFrom(source,amount, destination, 0.1f);
            }
        }
    }
}
