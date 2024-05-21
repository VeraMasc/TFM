using System;
using System.Collections.Generic;
using System.Collections;
using CardHouse;
using Common.Coroutines;
using UnityEngine;
using System.Linq;

namespace Effect{
    /// <summary>
    /// Clase madre de todos los tipos de habilidades.
    /// Las habilidades son efectos que tiene una carta a parte del principal
    /// </summary>
    [Serializable]
    public abstract class Ability
    {

        public string id;
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
                var routine =CardResolveOperator.singleton.triggerEffect(card,EffectChain.cloneFrom(effects), useActiveTriggers);
                yield return UCoroutine.Yield(routine);
            }
            else{
                Debug.LogError("Can't generate trigger from non-card");
            }

        }

        /// <summary>
        /// Gestiona los cambios de zona
        /// </summary>
        public virtual void onChangeZone(GroupName zone){

        }

        /// <summary>
        /// Zonas en las que se encuentran activas las habilidades
        /// </summary>
        public virtual IEnumerable<GroupName> activeZones{
            get=> new GroupName[]{GroupName.Board};
        }

        /// <summary>
        /// Marca si hay que usar triggers activos para la habilidad
        /// </summary>
        public virtual bool useActiveTriggers => false;

        /// <summary>
        /// Indica si la habilidad funciona en una zona concreta
        /// </summary>
        public virtual bool isActiveIn(GroupName zone){
            var zoneList = activeZones;

            if(zoneList.Any() && zoneList.First() == GroupName.None) //None indica que funciona siempre
                return true;

            return zoneList.Contains(zone);
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
        /// Lista de zonas en las que está disponible
        /// </summary>
        public List<GroupName> activeZoneList = new List<GroupName>(){GroupName.Board};
        public override IEnumerable<GroupName> activeZones{
            get=> activeZoneList;
        }

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
        /// Activa o desactiva el trigger de la habilidad al cambiar de zona según corresponda
        /// </summary>
        public override void onChangeZone(GroupName zone){
            if(isActiveIn(zone)){
                trigger.subscribe(source, listener);
            }
            else{
                //TODO: no desuscribir la carta entera
                trigger.unsubscribe(source);
            }
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

        public override bool useActiveTriggers => true;

        /// <summary>
        /// Velocidad de activación de la habilidad
        /// </summary>
        public SpeedTypes speed = SpeedTypes.Reaction;

        public virtual bool checkActivationTiming(Entity user){
            if(speed == SpeedTypes.Reaction)
                return true;

            return GameMode.current.isSpeedValid(user, speed);
        }

        /// <summary>
        /// Devuelve si se cumplen todas las condiciones para poder activarla
        /// </summary>
        public virtual bool canActivate (Entity user){
            //TODO: add mana requirements
            return checkActivationTiming(user);
        }
        public virtual IEnumerator activateAbility(Entity activator){
            //TODO: abilities with owner different than controller
            var context = new Context(source, activator);

            if(cost?.canBePaid(context) == false)
                yield break;
            
            if(cost!= null)
                yield return UCoroutine.Yield(cost.payCost(context));
            
            yield return UCoroutine.Yield(executeAbility(context));
        }
    }
}
