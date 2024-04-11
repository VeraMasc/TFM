using System;
using System.Collections;
using CustomInspector;
using UnityEngine;


namespace Effect.Value{
    /// <summary>
    /// Reusa un valor cualquiera
    /// </summary>
    [Serializable]
    public class ReuseValue:Value<object>
    {
        /// <summary>
        /// Posición del elemento desde el último índice
        /// </summary>
        public int index;

        /// <summary>
        /// Elimina la entrada del registro de valores
        /// </summary>
        public bool destructive;

        public override object getValue(Context context)
        {
            return ReuseValue.extractValue<object>(context, index, destructive);
        }

        /// <summary>
        /// Extrae un valor de la pila de valores guardados
        /// </summary>
        /// <param name="index">Posición del elemento desde el último índice</param>
        /// <param name="destructive">Elimina el valor tras extraerlo</param>
        /// <returns></returns>
        public static T extractValue<T>(Context context, int index, bool destructive){
            int size;
            if((size = context.previousValues.Count) <= index){
                Debug.LogError("Value to extract is out of bounds", (UnityEngine.Object) context.self);
                return default(T);
            }

            var pos = size-index;
            var val = context.previousValues[pos];

            if(destructive){
                context.previousValues.RemoveAt(pos);
            }

            if(val is T ret){
                return ret;
            }
            else{
                Debug.LogError($"Invalid extraction type for {val?.GetType()}", (UnityEngine.Object) context.self);
                return default(T);
            }
            
        }
    }

    /// <summary>
    /// Reusa un valor numérico
    /// </summary>
    [Serializable]
    public class ReuseNumeric:Numeric
    {
        /// <summary>
        /// Posición del elemento desde el último índice
        /// </summary>
        public int index;

        /// <summary>
        /// Elimina la entrada del registro de valores
        /// </summary>
        public bool destructive;

        public override int getValue(Context context)
        {
            return ReuseValue.extractValue<int>(context, index, destructive);
        }
    }
}