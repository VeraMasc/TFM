using System;
using System.Collections.Generic;
using CardHouse;
using UnityEngine;

namespace Effect{
    /// <summary>
    /// Contexto que se usa para resolver targets y efectos
    /// </summary>
    [Serializable]
    public class Context 
    {
        /// <summary>
        /// Refiere al propio objeto que produce el efecto
        /// </summary>
        public ITargetable self;

        /// <summary>
        /// Refiere a la fuente que causa el efecto
        /// (ES NECESARIO???)
        /// </summary>
        public ITargetable source;

        /// <summary>
        /// Entidad Dueña del efecto (controlador original de la carta)
        /// </summary>
        public Entity owner;

        /// <summary>
        /// Entidad que tiene el control del efecto/carta actualmente
        /// </summary>
        public Entity controller;

        /// <summary>
        /// Targets previos del proceso de resolución
        /// </summary>
        public List<ITargetable[]> previousTargets = new List<ITargetable[]>();
        
        /// <summary>
        /// Valores previos del proceso de resolución
        /// </summary>
        public List<object> previousValues = new List<object>();

        /// <summary>
        /// Indica a dónde irá la carta tras resolverse
        /// </summary>
        public CardGroup resolutionPile;

        /// <summary>
        /// Create context without owner or controller
        /// </summary>
        /// <param name="self">Object in question</param>
        public Context(ITargetable self){
            this.self = source = self; 
        }

        /// <summary>
        /// Create context with full detail
        /// </summary>
        public Context(ITargetable self, Entity controller, Entity owner=null, ITargetable source=null){
            this.self =self;
            this.source = source ?? self;
            this.controller = controller;
            this.owner = owner ?? controller;
        } 
    }
}
/// <summary>
/// Define qué objetos pueden ser targets.
/// Puede ser: Carta, Entidad, Grupo??? 
/// </summary>
public interface ITargetable{
    // public Transform targeterTransform;
    T GetComponent<T>();

    T GetComponentInChildren<T>();
}