using System;
using System.Collections.Generic;
using CardHouse;
using UnityEngine;


namespace Effect{
    /// <summary>
    /// Habilidad que da a una carta un cast alternativo
    /// </summary>
    [Serializable]
    public abstract class CastAbility : ActivatedAbility
    {
        
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
            if(isActiveIn(zone)){
                var entity = (source.data as ActionCard).effects.context?.controller;
                proxy = CardProxy.createProxy(source,entity);
                proxy.setAsActive();
            }
            else if(proxy){
                proxy.undoSeeking();
                proxy.DestroyCard();
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
    }
}