using UnityEngine;

using System;
using Effect.Value;
using System.Collections;
using System.Linq;
using Common.Coroutines;
using System.Collections.Generic;
using CardHouse;

namespace Effect{
    /// <summary>
    /// Pide al jugador que seleccione entre varias cartas a través de una interfaz modal
    /// </summary>
    [Serializable]
    public class SelectCardsFrom:Targeted, IValueEffect
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
                
                List<int> ret = null;

                //Generar diálogo modal
                yield return UCoroutine.Yield(GameUI.singleton.getInput(GameUI.singleton.prefabs.cardSelectInput, 
                obj => {
                    ret = (List<int>)obj;
                },
                new InputParameters{ values= targets.OfType<Card>().ToArray(), 
                    
                }));
                
            }
            
            
            yield break;
        }

        public static IEnumerator validatedChoice(IEnumerable<ITargetable> targets, Func<List<ITargetable>,bool> validator, Action<ITargetable[]> returnAction= null)
        {
            
            yield return UCoroutine.Yield(GameUI.singleton.getTargets(targets, 
                ()=> validator(GameUI.singleton.chosenTargets),
                returnAction,config:new InputParameters()));
        }

        public static IEnumerator amountChoice(IEnumerable<ITargetable> targets, int amount, Action<ITargetable[]> returnAction= null)
        {
            Func<List<ITargetable>,bool> validator = (List<ITargetable> chosen)=> chosen.Count == amount;
            yield return UCoroutine.Yield(validatedChoice(targets,validator,returnAction));
        }



        /// <summary>
        /// Crea un modal para poder seleccionar entre varias cartas
        /// </summary>
        /// <param name="card"></param>
        /// <param name="modes"></param>
        /// <returns></returns>
        public static IEnumerator selectModal(IEnumerable<Card> cards){
            
            List<int> ret = null;
            var setups = cards.Select(c=>c.data).OfType<MyCardSetup>();
            
            //Generar diálogo modal
            yield return UCoroutine.Yield(GameUI.singleton.getInput(GameUI.singleton.prefabs.cardSelectInput, 
            obj => {
                ret = (List<int>)obj;
            },
            new InputParameters{ values= cards.ToArray(), 
                
            }));
            
            

            //Usar resultado
            if(ret != null && ret.Count >0){
                var index = ret.First();
                Debug.Log(index);
            }
            
            
        }
    }
}