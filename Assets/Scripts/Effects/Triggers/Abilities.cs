using System;
using System.Collections.Generic;
using CardHouse;
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
        public List<IValueEffect> effects;
    }

    /// <summary>
    /// Son las habilidades que se activan de forma automática al ocurrir cosas concretas
    /// </summary>
    [Serializable]
    public class TriggeredAbility : Ability
    {
        /// <summary>
        /// Listener del trigger
        /// </summary>
        public Action listener;

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
