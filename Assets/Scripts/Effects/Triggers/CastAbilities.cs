using System;
using System.Collections.Generic;
using CardHouse;
using UnityEngine;


namespace Effect{
    /// <summary>
    /// Habilidad que da a una carta un cast alternativo
    /// </summary>
    [Serializable]
    public class CastAbility : ActivatedAbility
    {
        public override IEnumerable<GroupName> activeZones{
            get=> new GroupName[]{}; //No usa las zonas activas
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