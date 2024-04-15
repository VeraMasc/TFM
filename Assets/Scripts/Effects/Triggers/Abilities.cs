using System;
using System.Collections.Generic;
using System.Collections;
using CardHouse;
using Common.Coroutines;
using UnityEngine;

namespace Effect{
    /// <summary>
    /// Clase madre de todos los tipos de habilidades.
    /// Las habilidades son efectos que tiene una carta a parte del principal
    /// </summary>
    [Serializable]
    public abstract class Ability 
    {
        /// <summary>
        /// Carta que contiene la habilidad
        /// </summary>
        [HideInInspector]
        public Card source;
        /// <summary>
        /// Cadena de efectos a producir
        /// </summary>
        [SerializeReference, SubclassSelector]
        public List<EffectScript> effects;

        /// <summary>
        /// Ejecuta los efectos de la habilidad
        /// </summary>
        public IEnumerator executeAbility(Context context){
            var chain = EffectChain.cloneFrom(effects);
            foreach(var effect in chain.list){
                yield return UCoroutine.Yield(effect.execute(CardResolveOperator.singleton,context));
            }
        }
    }

    /// <summary>
    /// Son las habilidades que se activan de forma automática al ocurrir cosas concretas
    /// </summary>
    [Serializable]
    public class TriggeredAbility : Ability
    {
        /// <summary>
        /// Trigger a utilizar
        /// </summary>
        public BaseTrigger<object> trigger;
        /// <summary>
        /// Listener del trigger
        /// </summary>
        public Func<object,IEnumerator> listener;

        /// <summary>
        /// Ejecuta la habilidad con un parámetro inicial
        /// </summary>
        /// <param name="context">Contexto de ejecución</param>
        /// <param name="value">Valor inicial que insertar</param>
        /// <returns></returns>
        public IEnumerator executeAbility(Context context, object value){
            context.previousValues.Add(value);
            return executeAbility(context);
        }


        /// <summary>
        /// Activa o desactiva la habilidad al cambiar de zona según corresponda
        /// </summary>
        public void onChangeZone(){

        }
    }

    /// <summary>
    /// Son las habilidades que se activan de forma manual y suelen tener un coste
    /// </summary>
    [Serializable]
    public class ActivatedAbility : Ability
    {
        [SerializeReference,SubclassSelector]
        public ICost cost;
    }
}
