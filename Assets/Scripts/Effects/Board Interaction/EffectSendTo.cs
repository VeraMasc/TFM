using System;
using System.Collections;
using CardHouse;
using Common.Coroutines;
using UnityEngine;

namespace Effect{
    /// <summary>
    /// Efecto que manda una carta de una zona a otra
    /// </summary>
    [Serializable]
    public class SendTo : Targeted, IValueEffect
    {
        public GroupName zone;

        public MountingMode mode;

        public float duration = 0.1f;

        public override IEnumerator executeForeach(ITargetable target, CardResolveOperator stack, Context context)
        {
            
            if(target is Card card ){

                
                CardGroup group = null;
                var ownership = card.ownership;
                switch(zone){
                    case GroupName.Deck:
                        group = ownership?.owner?.deck;
                        break;
                    case GroupName.Hand:
                        group = ownership?.owner?.hand;
                        break;
                    case GroupName.Exile:
                        group = ownership?.owner?.exile;
                        break;
                    case GroupName.Lost:
                        group = ownership?.owner?.lost;
                        break;
                }

                //Destruir triggers
                if(ownership?.owner == null || card.data is TriggerCard){
                    card.Group?.UnMount(card);
                    GameObject.Destroy(card.gameObject);
                }

                if(group == null){
                    Debug.LogError($"Can't find groupName {zone} in entity {ownership.owner}", (UnityEngine.Object)(context?.self));
                    yield break;
                }

                if(card == stack.activeCard){ //Si es la carta activa, sobreescribir la resolution pile
                    context.resolutionPile = group;
                    yield break;
                }
                var index = group.getModeIndex(mode);
                group.Mount(card,index);
                yield return new WaitForSeconds(duration);
            }
            else{
                Debug.Log(target.GetType());
            }
        }

        public override IEnumerator execute(CardResolveOperator stack, Context context)
        {
            yield return UCoroutine.Yield(base.execute(stack, context));
            yield return new WaitForSeconds(0.3f);
        }
    }
}
