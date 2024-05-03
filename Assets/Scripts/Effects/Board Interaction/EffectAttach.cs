using System;
using System.Collections;
using System.Text.RegularExpressions;
using CardHouse;
using Common.Coroutines;
using CustomInspector;
using UnityEngine;

namespace Effect{
    /// <summary>
    /// Efecto que añade contenido a una habitación
    /// </summary>
    [Serializable]
    public class Attach : Targeted
    {
        


        public override IEnumerator execute(CardResolveOperator stack, Effect.Context context)
        {
            var targets = targeter.getTargets(context);
            //Check if valid
            if(!isValidResult(targets, context)){
                Debug.LogError("Invalid attachment targets", (UnityEngine.Object)context.self);
                yield break;
            }
            
            var target = targets[0];
            
            CardGroup group;
            if(target is Card card){
                group= card.attachedGroup;
            }else if(target is Entity entity){
                group= entity.attached;
            }
            else {
                Debug.LogError($"Attach target of type {target?.GetType()} is not suported", (UnityEngine.Object)context?.self);
                yield break;
            }

            
            if(context.effector is TriggerCard trigger){//Si es trigger, mover la carta directamente
                //TODO: no funciona. Arreglar
                var self = (Card)context.self;
                group.Mount(self);
                yield return UCoroutine.YieldAwait(()=>self.Homing.seeking);

                var zone = group.GetComponent<GroupZone>();
                if(zone){
                    yield return UCoroutine.Yield(zone.callEnterTrigger((Card)self));
                }
                
            }
            
            context.resolutionPile = group;
            
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


        public override bool isValidResult(ITargetable[] targets, Context context)
        {
            if(targets.Length != 1){
                return false; //No acepta más o menos de un target
            }
            var target = targets[0];
            
            return context.self != target; //No puede hacer attach a si mismo
        }


    }
}
