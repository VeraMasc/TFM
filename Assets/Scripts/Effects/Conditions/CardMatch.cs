using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace Effect.Condition{
    /// <summary>
    /// Comprueba si una carta coincide con una pool concreta de cartas
    /// </summary>
    [Serializable]
    public abstract class CardMatch:BaseCondition
    {
        /// <summary>
        /// Comprueba si se cumple la condición
        /// </summary>
        public virtual bool check(Effect.Context context){
            throw new NotImplementedException();
        }
    }
}