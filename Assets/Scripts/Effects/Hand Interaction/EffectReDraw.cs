using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CardHouse;
using Common.Coroutines;
using UnityEngine;


namespace Effect
{
    [Serializable]
    public class ReDraw : Targeted
    {
        /// <summary>
        /// Cartas a descartar
        /// </summary>
        [SerializeReference, SubclassSelector]
        public IValue cards;

       

        public override IEnumerator executeForeach(ITargetable target,CardResolveOperator stack, Context context)
        {
            if(target is Entity entity){
                if(cards.getValueObj(context) is IEnumerable<ITargetable> collection){
                    yield return UCoroutine.Yield(entity.reDraw(collection.OfType<Card>().ToArray()));
                }
                else{
                    Debug.Log(cards.getValueObj(context) );
                }
                
                
            }else{
                Debug.LogError($"Target of {this.GetType().Name} is \"{target.GetType().Name}\" not an entity", (UnityEngine.Object)context.self);
            }
        }

        /// <summary>
        /// Efécto básico de redraw de N cartas
        /// </summary>
        /// <returns></returns>
        public static IEnumerator basicRedraw(int amount, Entity entity){
            var cardsInHand = entity?.hand?.MountedCards;
            if((cardsInHand?.Count??0)<=0)
                yield break;

            Card[] cards = new Card[]{};
            yield return UCoroutine.Yield(GameUI.singleton.getTargets(cardsInHand, 
            ()=>{
                var chosen = GameUI.singleton.chosenTargets;
                return chosen.Count == amount;
            }, 
            (value)=>{cards= value.OfType<Card>().ToArray();
            },true));

            yield return UCoroutine.Yield(entity.reDraw(cards));
            yield break;
        }

    }
    
    
}