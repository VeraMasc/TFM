using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace Effect{
    [Serializable]
    public abstract class BaseCondition 
    {
        /// <summary>
        /// Comprueba si se cumple la condición
        /// </summary>
        public virtual bool check(object inputs, Context context){
            throw new NotImplementedException();
        }
    }
}