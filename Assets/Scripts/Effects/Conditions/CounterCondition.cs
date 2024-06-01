using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;


namespace Effect.Condition{
    /// <summary>
    /// Requiere que haya una cantidad concreta de algo
    /// </summary>
    [Serializable]
    public class CounterCondition:BaseCondition
    {

        public string counterType;
        [SerializeReference,SubclassSelector]
        public BaseCondition innerCondition = new AmountCondition();

        public override bool check(object inputs, Context context){
            if(inputs is ITargetable[] targets){
                return targets.All( t =>{ 
                    var counter = CounterHolder.getCounter(t,counterType);
                    return innerCondition.check(counter,context);
                });
            }
            else{
                Debug.LogError($"{inputs.GetType()} can't have counters");
            }
            return false;
        }
    }
}