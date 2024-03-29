using System;
using System.Collections;
using System.Collections.Generic;
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
    private GameObject _target;


    /// <summary>
    /// Obtiene el target resuelto (lo resuelve si es necesario)
    /// </summary>
    public GameObject getTarget(){
        if(_target == null){ //Resuelve el target si no lo ha hecho ya
            resolveTarget();
        }
        return _target;
    }

    /// <summary>
    /// Los targets no explícitos no se consideran targets a nivel mecánico,
    /// aunque funcionen prácticamente igual
    /// </summary>
    public virtual bool isExplicit{get=> false;}

    /// <summary>
    /// Sobreescribir este método para cambiar como se resuelven los targets
    /// </summary>
    
    public virtual void resolveTarget(){
        //TODO: implement targetting context
    }

}

[Serializable]
public class ContextualObjectTargeter:EffectTargeter
{
    public ContextualTargets contextual;
    public override void resolveTarget(){
        
    }

}

[Serializable]
public class ContextualEntityTarget:EffectTargeter
{
    public ContextualEntityTarget contextual;
    public override void resolveTarget(){
        
    }
}

/// <summary>
/// Targets contextuales de elementos del juego
/// </summary>
public enum ContextualTargets{
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