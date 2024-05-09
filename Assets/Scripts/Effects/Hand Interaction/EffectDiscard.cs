using System;
using System.Linq;
using System.Collections;
using Common.Coroutines;
using UnityEngine;
using CardHouse;
using System.Collections.Generic;


namespace Effect
{
    [Serializable]
    public class Discard : Targeted, ICost
    {
        /// <summary>
        /// Cartas a descartar
        /// </summary>
        [SerializeReference, SubclassSelector]
        public IValue cards;

        public bool canBePaid(Context context)
        {
            var toDiscard = cards.getValueObj(context);
            if(context.controller != null && toDiscard is IEnumerable<Card> collection){
                return collection.All( card => context.controller.hand.MountedCards.Contains(card));
            }
            return false;
        }

        public override IEnumerator executeForeach(ITargetable target,CardResolveOperator stack, Context context)
        {
            if(target is Entity entity){
                if(cards.getValueObj(context) is IEnumerable<ITargetable> collection){
                    yield return UCoroutine.Yield(entity.discard(collection.OfType<Card>().ToArray()));
                }
                else{
                    Debug.Log(cards.getValueObj(context) );
                }
                
                
            }else{
                Debug.LogError($"Target of {this.GetType().Name} is \"{target.GetType().Name}\" not an entity", (UnityEngine.Object)context.self);
            }
        }

        public IEnumerator payCost(Context context)
        {
            yield return UCoroutine.Yield(execute(CardResolveOperator.singleton, context));
        }
    }
}
