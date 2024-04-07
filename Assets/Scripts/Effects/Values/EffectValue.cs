using System;
using UnityEngine;


namespace Effect{
    /// <summary>
    /// Base de todos los valores párametro de los efectos
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class Value<T>
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
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual T getValue(){
            return value;
        }

        public virtual bool isDynamic{get=> false;}
    }
}

