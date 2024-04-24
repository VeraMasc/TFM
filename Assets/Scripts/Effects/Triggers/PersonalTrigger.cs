using System;
using System.Collections;
using System.Collections.Generic;
using CardHouse;
using Common.Coroutines;
using UnityEngine;
using UnityEngine.Events;

namespace Effect{
    
    /// <summary>
    /// Trigger de efecto personal sin parámetros
    /// </summary>
    [CreateAssetMenu(menuName = "Trigger/PersonalTrigger")]
    public class PersonalTrigger:BasePersonalTrigger<object>{
        
    }

    /// <summary>
    /// Trigger que depende no del objeto concreto sino de una o más entidades asociadas
    /// </summary>
    
    public class BasePersonalTrigger<T>:BaseTrigger<T>{

        /// <summary>
        /// Marca qué susciptores están asociados a cada entidad
        /// </summary>
        public Dictionary<Entity,HashSet<Card>> personalSubscribers = new Dictionary<Entity,HashSet<Card>>();

        /// <summary>
        /// Activa todos los listeners asociados a una entidad
        /// </summary>
        /// <param name="card">Carta que puede o no estar suscrita</param>
        public virtual IEnumerator invokeFor(Entity entity, T eventData = default(T)){
            
            if(!personalSubscribers.ContainsKey(entity)){
                // Debug.Log($"No event for {card.gameObject.name}",card.gameObject);
                yield break;
            }
            var cards = personalSubscribers[entity];

            foreach(var card in cards){

                if(!subscribers.ContainsKey(card)){
                    // Debug.Log($"No event for {card.gameObject.name}",card.gameObject);
                    yield break;
                }
            
            
            yield return UCoroutine.Yield(subscribers[card].Invoke(eventData));
            }

            
        }

        /// <summary>
        /// !NO USAR
        /// </summary>
        public override void subscribe(Card subscriber, Func<T, IEnumerator> listener)
        {
            base.subscribe(subscriber, listener);
        }

        /// <summary>
        /// !NO USAR
        /// </summary>
        /// <param name="subscriber"></param>
        public override void unsubscribe(Card subscriber)
        {
            base.unsubscribe(subscriber);
        }

        /// <summary>
        /// Suscribe una carta al evento de una o más entidades
        /// </summary>
        /// <param name="subscriber">carta a suscribir</param>
        /// <param name="persons">entidades a cuyo eventos suscribirse</param>
        /// <param name="listener">listener a suscribir</param>
        public virtual void subscribeFor(Card subscriber, IEnumerable<Entity> persons, Func<T,IEnumerator> listener){
            if(subscribers.ContainsKey(subscriber)){
                //TODO: controlar habilidades duplicadas
                return;
            }

            subscribers.Add(subscriber, listener);
            foreach(var entity in persons){
                //Crear hashset si no existe
                personalSubscribers.TryAdd(entity, new HashSet<Card>());
                //Añadir al hashset
                personalSubscribers[entity].Add(subscriber);
            }
            base.subscribe(subscriber,listener);
            
        }

        /// <summary>
        /// Desuscribe a la carta de todos los eventos de las personas mencionadas
        /// </summary>
        /// <param name="subscriber"></param>
        /// <param name="persons"></param>
        public virtual void unsubscribeFor(Card subscriber, IEnumerable<Entity> persons)
        {
            foreach(var entity in persons){
                if(!personalSubscribers.ContainsKey(entity))
                    continue;
                
                //eliminar del hashset
                personalSubscribers[entity].Remove(subscriber);
            }

            base.unsubscribe(subscriber);
        }
    }
}