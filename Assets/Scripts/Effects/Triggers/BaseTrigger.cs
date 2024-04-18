using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using CardHouse;
using Common.Coroutines;
using CustomInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Effect{



    //[CreateAssetMenu(menuName = "Trigger/BaseTrigger")]
    /// <summary>
    /// Clase madre de todos los triggers
    /// </summary>
    public abstract class BaseTrigger<T> :ScriptableObject
    {
        /// <summary>
        /// Cantidad de cartas suscritas
        /// </summary>
        [ReadOnly]
        public int count;

        /// <summary>
        /// Diccionario de suscriptores al evento
        /// </summary>
        public Dictionary<Card,Func<T,IEnumerator>> subscribers = new Dictionary<Card,Func<T,IEnumerator>>();

        /// <summary>
        /// Parámetros de la última llamada realizada a este evento
        /// </summary>
        public T eventData;



        /// <summary>
        /// Función para pruebas
        /// </summary>
        [NaughtyAttributes.Button("Invoke All")]
        private void invokeAll(){
            UCoroutine.Yield(invoke()).Start(GameController.singleton);
        }

        /// <summary>
        /// Invoca el evento usando los parámetros ya asignados
        /// </summary>
        public virtual IEnumerator invoke(){
            foreach(var key in subscribers.Keys){
                yield return UCoroutine.Yield(subscribers[key].Invoke(eventData));
            }
        }

        /// <summary>
        /// Invoca el evento con un parámetro
        /// </summary>
        public virtual IEnumerator invoke(T eventData){
            this.eventData = eventData;
            return invoke();
        }

        /// <summary>
        /// Suscribe una carta al evento
        /// </summary>
        /// <param name="subscriber">carta a suscribir</param>
        /// <param name="listener">listener a suscribir</param>
        public virtual void subscribe(Card subscriber, Func<T,IEnumerator> listener){
            if(subscribers.ContainsKey(subscriber)){
                //TODO: controlar habilidades duplicadas
                return;
            }
            subscribers.Add(subscriber, listener);
            count = subscribers.Count;
            
        }
        public virtual void unsubscribe(Card subscriber){
            if(subscribers.ContainsKey(subscriber)){
                
                subscribers.Remove(subscriber);
                count = subscribers.Count;
            }
        }
        
    }

    /// <summary>
    /// Trigger de efecto global (afecta a todos los objetos con el trigger por igual)
    /// </summary>
    // [CreateAssetMenu(menuName = "Trigger/GlobalTrigger")]
    public class GlobalTrigger<T> :BaseTrigger<T>
    {
        //TODO: Poner en archivo aparte
        public override IEnumerator invoke(){
            foreach(var key in subscribers.Keys){
                yield return UCoroutine.Yield(subscribers[key].Invoke(eventData));
            }
        }

        
    }

}
