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
        /// Genera un trigger con los efectos de la habilidad
        /// </summary>
        public virtual IEnumerator executeAbility(Context context){
            var self = context.self;
            if(self is Card card){
                var routine =CardResolveOperator.singleton.triggerEffect(card,EffectChain.cloneFrom(effects));
                yield return UCoroutine.Yield(routine);
            }

        }

        /// <summary>
        /// Zonas en las que se encuentran activas las habilidades
        /// </summary>
        public virtual IEnumerable<GroupName> activeZones{
            get=> new GroupName[]{GroupName.Board};
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
        public virtual IEnumerator executeAbility(Context context, object value){
            context.previousValues.Add(value);
            //TODO: Triggers for inputs not generating properly
            yield return UCoroutine.Yield( executeAbility(context));
        }


        /// <summary>
        /// Activa o desactiva la habilidad al cambiar de zona según corresponda
        /// </summary>
        public void onChangeZone(){

        }
    }

    
    /// <summary>
    /// Triggered Ability que no genera trigger en el stack
    /// </summary>
    [Serializable]
    public class HiddenTriggeredAbility : TriggeredAbility
    {

        /// <summary>
        /// Ejecuta los efectos de la habilidad sin crear trigger
        /// </summary>
        public override IEnumerator executeAbility(Context context){
            var chain = EffectChain.cloneFrom(effects);
            foreach(var effect in chain.list){
                yield return UCoroutine.Yield(effect.execute(CardResolveOperator.singleton,context));
            }
        }
    }

    /// <summary>
    /// Son las habilidades que se activan de forma manual y suelen tener un coste
    /// </summary>
    [Serializable]
    public class ActivatedAbility : Ability, IActionable
    {
        [SerializeReference,SubclassSelector]
        public ICost cost;

        public virtual IEnumerator activateAbility(Entity activator){
            //TODO: abilities with owner different than controller
            var context = new Context(source, activator);

            if(!cost.canBePaid(context))
                yield break;
            
            cost.payCost(context);
            
            yield return UCoroutine.Yield(executeAbility(context));
        }
    }
}
