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
        /// Indica si el trigger genera logs al ser llamado
        /// </summary>
        public bool produceLogs;

        public UnityEvent events;


        /// <summary>
        /// Función para pruebas
        /// </summary>
        [NaughtyAttributes.Button("Invoke All")]
        protected void invokeAll(){
            events.AddListener(()=>Debug.Log("Test"));
            UCoroutine.Yield(invoke()).Start(GameController.singleton);
            events.Invoke();
        }

        /// <summary>
        /// Invoca el evento usando los parámetros ya asignados
        /// </summary>
        public virtual IEnumerator invoke(){
            var resolver = CardResolveOperator.singleton;
            generateLog();
            foreach(var key in subscribers.Keys.ToArray()){
                
                yield return resolver.StartCoroutine(subscribers[key].Invoke(eventData));
                Debug.Log($"Finished invoking trigger {key}",key);
            }
        }

        /// <summary>
        /// Invoca el evento con un parámetro
        /// </summary>
        public virtual IEnumerator invoke(T eventData){
            this.eventData = eventData;
            events.Invoke();
            yield return invoke();
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

        /// <summary>
        /// Genera el log
        /// </summary>
        public virtual void generateLog(object obj=null){
            if(!produceLogs)
                return;
            var message = $"Invoking {this.name}";
            if(obj!=null){
                message += $" with {obj.ToString()}";
            }
            Debug.Log(message);
        }
        
        public virtual void OnEnable()
        {
            subscribers = new();
            count=0;
        }

        public virtual void OnValidate() {
            count = subscribers.Count;
        }
    }

    
    public abstract class GlobalTrigger<T> :BaseTrigger<T>
    {
        //TODO: Poner en archivo aparte
        public override IEnumerator invoke(){
            var resolver = CardResolveOperator.singleton;
            generateLog();
            events.Invoke();
            foreach(var key in subscribers.Keys.ToArray()){
                yield return resolver.StartCoroutine(subscribers[key].Invoke(eventData));
            }
           
        }
        
        
    }
  
}
