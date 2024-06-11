using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;


namespace Effect.Condition{
    /// <summary>
    /// Comprueba si el valor coincide con alguno de los especificados
    /// </summary>
    [Serializable]
    public class Matches:BaseCondition
    {
        [SerializeReference, SubclassSelector]
        public IValue targeter;
        /// <summary>
        /// Comprueba si se cumple la condición
        /// </summary>
        public override bool check(object inputs, Context context){
            var obj = targeter.getValueObj(context);
            object[] objects = obj is IEnumerable<object> coll? coll.ToArray(): new object[]{obj};

            if(inputs is IEnumerable<object> collection){
                return collection.Intersect(objects).Any();
            }
            else{
                return objects.Contains(inputs);
            }
        }
    }
}