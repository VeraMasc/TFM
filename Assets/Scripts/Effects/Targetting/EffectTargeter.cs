using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CardHouse;
using CustomInspector;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Clase madre de todos los tipos de targets que puede tener un efecto
/// </summary>
[Serializable]
public abstract class EffectTargeter 
{
    [SerializeField,ReadOnly]
    protected ITargetable[] _targets;

    /// <summary>
    /// Atajo para asignar un solo target
    /// </summary>
    protected ITargetable singleTarget{
        set =>_targets = new ITargetable[1]{value};
    }

  
    /// <summary>
    /// Obtiene el target resuelto (lo resuelve si es necesario)
    /// </summary>
    public ITargetable[] getTargets(Effect.Context context){
        if(_targets == null){ //Resuelve el target si no lo ha hecho ya
            resolveTarget(context);
            storeTargets(context);
        }
        return _targets;
    }

    /// <summary>
    /// Almacena los targets para su posible reuso
    /// </summary>
    public virtual void storeTargets(Effect.Context context){
        context.previousTargets.Add(_targets);
    }

    /// <summary>
    /// Los targets no explícitos no se consideran targets a nivel mecánico,
    /// aunque funcionen prácticamente igual
    /// </summary>
    public virtual bool isExplicit{get=> false;}

    /// <summary>
    /// Sobreescribir este método para cambiar como se resuelven los targets
    /// </summary>
    public virtual void resolveTarget(Effect.Context context){
        throw new NotImplementedException();
    }

    /// <summary>
    /// Sobreescribir este método para cambiar qué targets individuales se consideran válidos
    /// </summary>
    public virtual bool isValidTarget(ITargetable target,Effect.Context context){
        return true;
    }

    /// <summary>
    /// Sobreescribir este método para cambiar qué conjunto global de targets se considera válido
    /// </summary>
    public virtual bool isValidResult(ITargetable[] targets,Effect.Context context){
        return true;
    }

    /// <summary>
    /// Obtiene todas las partes del targeter 
    /// </summary>
    /// <returns> lista de los targeters</returns>
    public virtual List<EffectTargeter> getTargeterNodes(){
        return new List<EffectTargeter>(){this};
    }

    /// <summary>
    /// Obtiene el ITargetable padre de un ITargetable
    /// </summary>
    public static ITargetable getObjParent(ITargetable obj){
        if(obj is Card card){
            var parent = card.Group.transform.parent.GetComponent<ITargetable>();
            return parent;
        }
        
        return null;
    }

}

[Serializable]
public class ContextualObjectTargeter:EffectTargeter
{
    public ContextualObjTargets contextual;
    public override void resolveTarget(Effect.Context context){
        switch(contextual){
            case ContextualObjTargets.self:
                singleTarget= context.self;
                break;
            
            case ContextualObjTargets.parent:
                singleTarget =getObjParent(context.self);
                break;
            
            case ContextualObjTargets.all:
                _targets = GameController.singleton.getTargetablesOnBoard()
                    .Where(t=> isValidTarget(t,context)).ToArray();
                break;
            default:
                throw new NotImplementedException();
        }
    }

}

/// <summary>
/// Targets entities by context
/// </summary>
[Serializable]
public class ContextualEntityTargeter:EffectTargeter
{
    public ContextualEntityTargets contextual;
    public override void resolveTarget(Effect.Context context){
        switch(contextual){
            case ContextualEntityTargets.controller:
                singleTarget= context.controller;
                break;
            
            case ContextualEntityTargets.owner:
                singleTarget = context.owner;
                break;
            
            case ContextualEntityTargets.allies:
                _targets = GameController.singleton.getMembers(context.controller.team).ToArray();
                break;

            case ContextualEntityTargets.everyone:
                _targets = GameController.singleton.entities.ToArray();
                break;

            case ContextualEntityTargets.opponents:
                _targets =  GameController.singleton.getEnemies(context.controller.team).ToArray();
                break;
            case ContextualEntityTargets.attachedTo:
                if (context.self is Card card){
                    var group = card.Group?.GetComponent<GroupZone>();
                    if(group?.owner){
                        singleTarget = group.owner;
                    }
                }
                
                break;
        }
        _targets ??= new ITargetable[0];
    }
}

/// <summary>
/// Targets contextuales de elementos del juego
/// </summary>
public enum ContextualObjTargets{
    self,
    /// <summary>
    /// Jugador u objeto al que está enganchado
    /// </summary>
    parent,
    children,
    siblings,
    all,
    others,
    
}

public enum ContextualEntityTargets{
    controller,
    owner,
    allies,
    opponents,
    everyone,
    attachedTo
}