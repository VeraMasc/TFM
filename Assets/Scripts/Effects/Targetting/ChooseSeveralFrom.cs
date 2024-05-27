using UnityEngine;

using System;
using Effect.Value;
using System.Collections;
using System.Linq;
using Common.Coroutines;
using System.Collections.Generic;

namespace Effect{
    /// <summary>
    /// Pide al jugador que seleccione un target entre las opciones ofrecidas
    /// </summary>
    [Serializable]
    public class ChooseSeveralFrom:Targeted, IValueEffect
    {

        /// <summary>
        /// Condición requerida para aceptar el input
        /// </summary>
        [SerializeReference, SubclassSelector]
        public BaseCondition condition;

        public override IEnumerator execute(CardResolveOperator stack, Context context)
        {
            var targets = targeter.getTargets(context);
            var pos = context.previousTargets.Count-1;
            if(pos>=0){
                var config = new InputParameters(){
                    context=context
                };
                //Get player input
                GameUI.singleton.possibleTargets= targets.ToArray();
                yield return UCoroutine.Yield(GameUI.singleton.getTargets(targets, 
                ()=>{
                    var chosen = GameUI.singleton.chosenTargets;
                    return condition?.check(chosen, context) ?? true;
                }, 
                (value)=>{
                    context.previousTargets[pos]= value;
                    context.previousChosenTargets.Add(value);
                },config:config));
                
            }
            
            
            yield break;
        }

        public static IEnumerator validatedChoice(IEnumerable<ITargetable> targets, Func<List<ITargetable>,bool> validator, Action<ITargetable[]> returnAction= null,bool canCancel=true)
        {
            
            yield return UCoroutine.Yield(GameUI.singleton.getTargets(targets, 
                ()=> validator(GameUI.singleton.chosenTargets),
                returnAction, config:new InputParameters(){
                    canCancel=canCancel
                }));
        }

        public static IEnumerator amountChoice(IEnumerable<ITargetable> targets, int amount, Action<ITargetable[]> returnAction= null, bool canCancel=true)
        {
            Func<List<ITargetable>,bool> validator = (List<ITargetable> chosen)=> chosen.Count == amount;
            yield return UCoroutine.Yield(validatedChoice(targets,validator,returnAction,canCancel));
        }
    }
}