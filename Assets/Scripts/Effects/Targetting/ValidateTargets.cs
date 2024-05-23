using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CardHouse;
using Common.Coroutines;
using UnityEngine;

namespace Effect
{
    /// <summary>
    /// Comprueba si los targets son válidos
    /// </summary>
    [Serializable]
    public class ValidateTargets: EffectScript, IValueEffect, IPrecalculable
    {
        public bool requireAll;
        public List<int> targetIndexes = new(){0};
        public List<Tuple<ITargetable[],double[]>> entryTimes = new List<Tuple<ITargetable[], double[]>>();

        public override IEnumerator execute(CardResolveOperator stack, Effect.Context context){

            if(context.precalculated == false){
                Debug.Log($"Validating {targetIndexes.Count}");
                foreach(var index in targetIndexes){
                    var values = context.previousTargets[index];
                    storeTargets(values);
                }
  
            }else{
                validate(context);
            }          
            
            yield break;
        }

        /// <summary>
        /// Guarda cada target con su entrytime correspondiente
        /// </summary>
        /// <param name="targets"></param>
        public void storeTargets(ITargetable[] targets){
            var times = targets.Select(t =>{
                if(t is Card card && card.data is MyCardSetup setup){
                    return  setup?.effects?.entryTime ?? 0;
                }

                //Ignorar el entry time si no es una carta
                return 0;
            });

            entryTimes.Add(new Tuple<ITargetable[],double[]>(targets,times.ToArray()));
        }
    

        public IEnumerator precalculate(CardResolveOperator stack, Context context)
        {
            yield return execute(stack,context).Start(stack);
        }

        public void validate(Context context){
            
            foreach ((var targets, var times) in entryTimes){
                
                var index = context.previousTargets.IndexOf(targets);

                var tuples = targets.Select((t,i)=> 
                    new Tuple<ITargetable,double>(t,times[i])
                ).ToArray();

                var validated = excludeInvalid(tuples);
                if(validated.Length < targets.Length){
                    context.previousTargets[index] = validated; //Replace with validated values

                    //Cancelar ejecución si no hay suficientes valores válidos
                    if(validated.Length ==0 || requireAll){
                        Debug.Log("Invalid target found");
                        context.mode = ExecutionMode.cancel;
                    }
                }
                
            }
        }

        public ITargetable[] excludeInvalid(Tuple<ITargetable,double>[] targets){

            
            return targets.Where(tuple =>{
                var ( target, time) = tuple;
                if(target is Card card && card.data is MyCardSetup setup){
                    return setup?.effects?.entryTime == time;
                }
                return time ==0; //Devolver true si no importaba el tiempo
            })
            .Select(t => t.Item1)
            .ToArray();
        }
    }
}

