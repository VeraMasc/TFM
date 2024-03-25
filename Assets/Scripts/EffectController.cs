using System.Collections;
using System.Collections.Generic;
using CardHouse;
using UnityEngine;
using System.Linq;
using System;

using Unity.VisualScripting;
using CustomInspector;

/// <summary>
/// Gestiona la ejecución de los efectos de las cartas
/// </summary>
[RequireComponent(typeof(Card))]
public class EffectController : MonoBehaviour
{
    [SelfFill]
    public ScriptMachine scriptMachine;

    /// <summary>
    /// Prepara el cast del efecto, pide targets, etc
    /// </summary>
    [NaughtyAttributes.Button]
    public void prepareCast(){

    }

    /// <summary>
    /// Resuelve el efecto como tal
    /// </summary>
    [NaughtyAttributes.Button]
    public void resolveEffect(){
        CustomEvent.Trigger(gameObject,"OnEffectResolve");
        
    }
    
     
}

/// <summary>
/// Indica qué hacer con la carta cuando se resuelve
/// </summary>
public enum ResolutionMode{
    discard,
    exile,
    destroy,
    toHand,
    other
}