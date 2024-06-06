using System;
using System.Collections;
using System.Collections.Generic;
using CardHouse;
using Common.Coroutines;
using UnityEngine;


namespace Effect{
    /// <summary>
    /// Habilidad que da a una carta un cast alternativo
    /// </summary>
    [Serializable]
    public abstract class CastAbility : ActivatedAbility
    {
        public override IEnumerator executeAbility(Context context){
            //Execute pre-cast effects
            var chain = EffectChain.cloneFrom(effects);
            foreach(var effect in chain.list){
                yield return UCoroutine.Yield(effect.execute(CardResolveOperator.singleton,context));
            }
            
            yield return CardResolveOperator.singleton.castCard(source,cost)
                .Start(source);
        }
    }

    /// <summary>
    /// Habilidad de cast que depende de la zona
    /// </summary>
    [Serializable]
    public class ZoneCastAbility : CastAbility
    {
        /// <summary>
        /// Lista de zonas en las que está disponible
        /// </summary>
        public List<GroupName> activeZoneList = new List<GroupName>(){GroupName.Board};
        public override IEnumerable<GroupName> activeZones{
            get=> activeZoneList;
        }

        public CardProxy proxy;

        public override void onChangeZone(GroupName zone)
        {
            if(isActiveIn(zone) && zone != GroupName.Hand){
                var entity = (source.data as ActionCard).effects.context?.controller;
                proxy ??= CardProxy.createProxy(source,entity);
                proxy.setAsActive();
                //Hacer que el grupo se recalcule
                UCoroutine.Yield(new WaitForEndOfFrame())
                .Then(()=> (proxy.Group ?? proxy.fakedGroup).ApplyStrategy())
                .Start(source);
            }
            else if(proxy){
                proxy.undoSeeking();
                proxy.DestroyCard();
                proxy=null;
            }
        }

    }

    [Serializable]
    public class UniversalCastAbility : CastAbility
    {
        public override IEnumerable<GroupName> activeZones{
            get=> new GroupName[]{GroupName.None}; //Funciona en todas las zonas
        }
        public override bool isActiveIn(GroupName zone){
            return true; //Siempre está activa (no depende de la zona, sino del poder usar la carta)
        }
    }


    /// <summary>
    /// Como las habilidades activas, pero se pueden activar desde otras zonas
    /// </summary>
    [Serializable]
    public class ActivatedZoneAbility:ActivatedAbility
    {
        /// <summary>
        /// Lista de zonas en las que está disponible
        /// </summary>
        public List<GroupName> activeZoneList = new();
        public override IEnumerable<GroupName> activeZones{
            get=> activeZoneList;
        }

        public override IEnumerator activateAbility(Entity activator){
            //TODO: abilities with owner different than controller
           
            var context = new Context(source, activator, source.ownership?.owner);

            if(cost?.canBePaid(context) == false)
                yield break;
            
            if(source.data is MyCardSetup setup){
                setup.effects.paidCost = cost;
            }
            yield return UCoroutine.Yield(executeAbility(context));
        }
    }
}