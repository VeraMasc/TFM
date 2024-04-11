using System;
using UnityEngine;


namespace Effect{
    /// <summary>
    /// Base de todos los valores párametro de los efectos
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class Value<T>:IValue
    {
        public Value(){

        }

        public Value(T init){
            value = init;
        }
        /// <summary>
        /// Valor final del parámetro
        /// </summary>
        [SerializeField]
        protected T value;

        /// <summary>
        /// Obtiene el valor
        /// </summary>
        /// <returns></returns>
        public virtual T getValue(Context context){
            return value;
        }

        public virtual object getValueObj(Context context){
            return getValue(context);
        }

        public virtual bool isDynamic{get=> false;}
    }

    public interface IValue{
        public object getValueObj(Context context);
    }
}

