using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CardHouse;
using CustomInspector;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Clase madre de todos los tipos de targets que puede tener un efecto
/// </summary>
[Serializable]
public abstract class EffectTargeter : IClonableEffectElement
{
    [SerializeField,ReadOnly]
    protected ITargetable[] _targets;

    /// <summary>
    /// Atajo para asignar un solo target
    /// </summary>
    protected ITargetable singleTarget{
        set =>_targets = new ITargetable[]{value};
    }

    public IClonableEffectElement clone(){
        return EffectScript.cloneScriptObj(this);
    }
    /// <summary>
    /// Obtiene el target resuelto (lo resuelve si es necesario)
    /// </summary>
    public ITargetable[] getTargets(TargettingContext context){
        if(_targets == null){ //Resuelve el target si no lo ha hecho ya
            resolveTarget(context);
            context.previousTargets.Add(_targets);
        }
        return _targets;
    }

    /// <summary>
    /// Los targets no explícitos no se consideran targets a nivel mecánico,
    /// aunque funcionen prácticamente igual
    /// </summary>
    public virtual bool isExplicit{get=> false;}

    /// <summary>
    /// Sobreescribir este método para cambiar como se resuelven los targets
    /// </summary>
    
    public virtual void resolveTarget(TargettingContext context){
        //TODO: implement targetting context
        throw new NotImplementedException();
    }

    /// <summary>
    /// Obtiene el ITargetable padre de un ITargetable
    /// </summary>
    public static ITargetable getObjParent(ITargetable obj){
        if(obj is Card){
            Card card = (Card) obj;
            var parent = card?.Group?.GetComponent<ITargetable>();
            return parent;
        }
        
        return null;
    }

}

[Serializable]
public class ContextualObjectTargeter:EffectTargeter
{
    public ContextualObjTargets contextual;
    public override void resolveTarget(TargettingContext context){
        switch(contextual){
            case ContextualObjTargets.self:
                singleTarget= context.self;
                break;
            
            case ContextualObjTargets.parent:
                singleTarget =getObjParent(context.self);
                break;
            
            case ContextualObjTargets.all:
                _targets = GameController.singleton.getAllOfType(context);
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
    public override void resolveTarget(TargettingContext context){
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
        }
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
    all,
    others
}

public enum ContextualEntityTargets{
    controller,
    owner,
    allies,
    opponents,
    everyone
}