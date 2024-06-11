using UnityEngine;

using System;
using Effect.Value;
using System.Collections;
using System.Linq;
using Common.Coroutines;
using CardHouse;

namespace Effect{
    /// <summary>
    /// Pide al jugador que seleccione un target entre las opciones ofrecidas
    /// </summary>
    [Serializable]
    public class ChooseTargetFrom:Targeted, IValueEffect
    {

        public override IEnumerator execute(CardResolveOperator stack, Context context)
        {
            
            var targets = targeter.getTargets(context);
            var pos = context.previousTargets.Count-1;
            if(pos>=0){
                focusActiveCards(targets);
                
                var config = new InputParameters(){
                    context=context,
                    text = "Choose one"
                };
                
                if(!context?.controller?.AI){
                    GameUI.singleton.possibleTargets= targets.ToArray();
                    //Get player inputs
                    yield return UCoroutine.Yield(GameUI.singleton.getTargets(targets, 
                    ()=>{
                        return GameUI.singleton.chosenTargets?.Count == 1;
                    }, 
                    (value)=>{
                        context.previousTargets[pos]= value;
                        context.previousChosenTargets.Add(value);
                    }, true,config));
                }
                else{
                    //Get AI input
                    var value = context.controller.AI.chooseTargets(targets,new ChoiceInfo(){
                        context = context,
                        amount=1,
                        cancellable=false,
                    });
                    context.previousTargets[pos]= value;
                    context.previousChosenTargets.Add(value);
                    GameUI.singleton.viewFocusedTargeting(null);
                }
                

                context.choiceTreeIncrease();
                
                
            }
            
            
            yield break;
        }
    }
}