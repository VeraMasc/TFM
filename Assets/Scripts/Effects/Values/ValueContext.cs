using System.Collections.Generic;
using UnityEngine;
using System;


namespace Effect.Value{
    /// <summary>
    /// Contexto que se usa para resolver targets y efectos
    /// </summary>
    [Serializable]
    public class Context 
    {
        /// <summary>
        /// Contiene el targetting context
        /// </summary>
        public TargettingContext targetting;

        /// <summary>
        /// Valores previos del proceso de resolución
        /// </summary>
        public List<object[]> previousValues = new List<object[]>();

        public Context(){}
        public Context(TargettingContext tContext){
            targetting=tContext;
        }

        
    }

}