using System;
using System.Collections.Generic;
using System.Linq;
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
        /// Se refiere a la carta o al trigger que ejecuta el efecto en el stack
        /// </summary>
        public ITargetable effector;

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
        /// Targets escogidos previamente que mostrar en la interfaz
        /// </summary>
        public List<ITargetable[]> previousChosenTargets = new List<ITargetable[]>();
        
        /// <summary>
        /// Valores previos del proceso de resolución
        /// </summary>
        public List<object> previousValues = new List<object>();

        /// <summary>
        /// Estructura de las decisiones tomadas hasta ahora
        /// </summary>
        public List<int> choiceTreePath = new List<int>{0};

        public int choiceTreeIncrease(int amount=1){
            choiceTreePath[choiceTreePath.Count-1] += amount;
            Debug.Log(string.Join(", ",choiceTreePath));
            return choiceTreePath.Last();
        }

        public void choiceTreeDeepen(int parentIndex){
            choiceTreePath[choiceTreePath.Count-1] = parentIndex;
            choiceTreePath.Add(0);
        }

        public int choiceTreePop(){
            var val = choiceTreePath[choiceTreePath.Count-1];
            choiceTreePath.RemoveAt(choiceTreePath.Count-1);
            return val;
        }

        /// <summary>
        /// Indica a dónde irá la carta tras resolverse
        /// </summary>
        public CardGroup resolutionPile;

        /// <summary>
        /// Indica si ha sido precalculado o no
        /// </summary>
        public bool precalculated;

        /// <summary>
        /// Modo de ejecución
        /// </summary>
        public ExecutionMode mode;

        
        /// <summary>
        /// Create context without owner or controller
        /// </summary>
        /// <param name="self">Object in question</param>
        public Context(ITargetable self, ITargetable effector =null){
            this.self = self;
            this.effector = effector ?? self;
        }

        /// <summary>
        /// Create context with full detail
        /// </summary>
        public Context(ITargetable self, Entity controller, Entity owner=null, ITargetable effector=null){
            this.self =self;
            this.effector = effector ?? self;
            this.controller = controller;
            this.owner = owner ?? controller;
        }

        /// <summary>
        /// Copia el contexto sin datos de ejecución (previousValues y previousTargets)
        /// </summary>
        /// <param name="original"></param>
        public Context(Context original):this(original.self,original.controller,original.owner,original.effector){

        }

    
        public Context(Card self, CardOwnership ownership, Card effector = null):this(self, ownership?.controller, ownership?.owner,effector){
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

    public Transform targeterTransform {get;}

    T GetComponentInChildren<T>();

    public SpriteRenderer outlineRenderer {get;}
}

/// <summary>
/// Indica cómo gestionar la ejecución del contexto
/// </summary>
public enum ExecutionMode{
    normal,
    /// <summary>
    /// Indica que la ejecución se ha de cancelar
    /// </summary>
    cancel
}