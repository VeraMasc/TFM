using System;
using System.Collections;
using System.Collections.Generic;
using CardHouse;
using Common.Coroutines;
using UnityEngine;
using UnityEngine.Events;


namespace Effect{

    /// <summary>
    /// Clase Base de los triggers locales
    /// </summary>
    public class BaseLocalTrigger<T> :BaseTrigger<T>
    {
        

        

        /// <summary>
        /// Activa el listener del objeto si está suscrito
        /// </summary>
        /// <param name="card">Carta que puede o no estar suscrita</param>
        public virtual IEnumerator invokeOn(Card card, T eventData = default(T)){
            
            if(subscribers.ContainsKey(card)){
                yield return UCoroutine.Yield(subscribers[card].Invoke(eventData));
            }
            else{ 
                // Debug.Log($"No event for {card.gameObject.name}",card.gameObject);
            }
        }

        
    }

}