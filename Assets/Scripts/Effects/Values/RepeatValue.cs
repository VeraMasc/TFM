using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Common.Coroutines;
using CustomInspector;
using UnityEngine;


namespace Effect.Value{
    /// <summary>
    /// Repite un valor varias veces
    /// </summary>
    [Serializable]
    public class Repeat:Value<object>
    {
        /// <summary>
        /// Cuantas veces repetir el valor
        /// </summary>
        [SerializeReference,SubclassSelector]
        public IValue times;

        [SerializeReference,SubclassSelector]
        public IValue input;

        public override object getValueObj(Context context)
        {
            var repeat = (int)times.getValueObj(context);
            var obj = input?.getValueObj(context);
            if(obj is IEnumerable<object> array){
                Type elementType = array.GetType().GetGenericArguments().Single();
                Debug.Log(elementType);
                var generic =  typeof(Repeat).GetMethod(nameof(Repeat.repeatArrayValue))
                    .MakeGenericMethod(elementType);
                return generic.Invoke(null, new object[] {array,repeat});
            }else{
                Debug.Log(obj);
                Type objType = obj.GetType();
                var generic =  typeof(Repeat).GetMethod(nameof(Repeat.repeatValue))
                    .MakeGenericMethod(objType);
                return generic.Invoke(null, new object[] {obj,repeat});
            }
            
        }

        public static T[] repeatArrayValue<T>(IEnumerable<T> array,int repeat){
            return Enumerable.Repeat(array, repeat)
                    .SelectMany(a => a.ToArray())
                    .Cast<T>().ToArray();
        }

        public static T[] repeatValue<T>(T obj,int repeat){
            return Enumerable.Repeat(obj, repeat)
                    .Select(v => v)
                    .Cast<T>().ToArray();
        }
    }
}