using System;
using System.Collections;
using UnityEngine;


namespace Effect
{
    [Serializable]
    public class DiscardRandom : Targeted, ICost
    {
        /// <summary>
        /// Cantidad a descartar
        /// </summary>
        [SerializeReference, SubclassSelector]
        public Value.Numeric amount;


        public bool canBePaid(Context context)
        {
            if(context.controller != null){
                var cardsInHand = context.controller.hand.MountedCards.Count;
                return cardsInHand >= amount.getValue(context);
            }
            return false;
        }

        public override IEnumerator executeForeach(ITargetable target,CardResolveOperator stack, Context context)
        {
            if(target is Entity entity){
                yield return entity.discardRandom(amount.getValue(context));
                
            }else{
                Debug.LogError($"Target of {this.GetType().Name} is \"{target.GetType().Name}\" not an entity", (UnityEngine.Object)context.self);
            }
        }
    }
}
