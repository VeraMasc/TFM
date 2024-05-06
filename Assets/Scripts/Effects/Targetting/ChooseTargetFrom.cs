using UnityEngine;

using System;
using Effect.Value;
using System.Collections;
using System.Linq;
using Common.Coroutines;

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
                
                GameUI.singleton.possibleTargets= targets.ToArray();
                Debug.Log(GameUI.singleton.possibleTargets.Count());
                yield return UCoroutine.Yield(GameUI.singleton.getTargets(targets, 
                ()=>{
                    return GameUI.singleton.chosenTargets?.Count == 1;
                }, 
                (value)=>{
                    Debug.Log(value.FirstOrDefault());
                    context.previousTargets[pos]= value;
                    context.previousChosenTargets.Add(value);
                }));
                
            }
            
            
            yield break;
        }
    }
}