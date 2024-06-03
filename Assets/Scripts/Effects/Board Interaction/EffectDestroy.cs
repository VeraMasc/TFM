using System;
using System.Collections;
using System.Text.RegularExpressions;
using CardHouse;
using Common.Coroutines;
using UnityEngine;

namespace Effect{
    /// <summary>
    /// Efecto que destruye permanentes o counterea acciones o triggers
    /// </summary>
    [Serializable]
    public class Destroy : Targeted
    {


        public override IEnumerator executeForeach(ITargetable target, CardResolveOperator stack, Context context)
        {
            if(target is Card card && card.data is not TriggerCard)
            {
                var zone = card.Group.GetComponent<GroupZone>();
                if(zone.zone != GroupName.Board && zone.zone !=GroupName.Stack){
                    yield break; //No puede destruir objetos que no están en el campo o el stack
                }

                var manager = GameController.singleton.triggerManager;
                yield return card.StartCoroutine(manager.onDestroy.invokeOn(card));
                var ownership = card.ownership;
                if(ownership){
                    yield return card.StartCoroutine(CardTransferOperator.sendCard(card,ownership.owner.discarded));
                }else{
                    UCoroutine.Yield(stack.waitTillOpen)
                    .Then(()=>card.DestroyCard())
                    .Start(card);
                }

            }
        }
    }
}
