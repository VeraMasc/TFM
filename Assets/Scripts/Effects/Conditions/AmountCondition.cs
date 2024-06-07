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
    public class AmountCondition:BaseCondition
    {
        
        public int min=0;
        public int max=2;

        public override bool check(object inputs, Context context){
            //Cast to collection of elements
            if(inputs is IEnumerable<object> collection){
                var amount = collection.Count();
                return amount <=max && amount >= min;
            }
            else if(inputs is int integer){
                return integer <=max && integer >= min;
            }

            Debug.LogError($"Type {inputs?.GetType()} not accepted in condition{this.GetType().Name}");
            return false;
        }
        

        public override string desctiption(Context context){
            if(min == 0)
                return $"up to {max}";

            return $"between {min} and {max}";
        }
    }
}