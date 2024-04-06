using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CustomInspector;
using UnityEngine;

/// <summary>
/// Identifica una cadena de efectos
/// </summary>
[Serializable]
public class EffectChain: IClonableEffectElement
{
    [SerializeReference,SubclassSelector]
    public List<EffectScript> list;

    /// <summary>
    /// Comprueba si no hay efectos para esta cadena
    /// </summary>
    public bool isEmpty{get => list.Count==0;}

    /// <summary>
    /// Tamaño de la cadena de efectos
    /// </summary>
    public int size{get => list.Count;}

    public EffectChain( List<EffectScript> effects){
        list=effects;
    }

    
    /// <summary>
    /// Copia la cadena de efectos
    /// </summary>
    public IClonableEffectElement clone(){
        return EffectScript.cloneScriptObj(this);
    }

    /// <summary>
    /// Copia la secuencia de efectos como una cadena independiente
    /// </summary>
    public EffectChain cloneFrom(IEnumerable<EffectScript> source){
        throw new NotImplementedException();
        //TODO: implementar clonación de efectos
    }
}
/// <summary>
/// Indica cada pieza de la cadena de efectos
/// </summary>
[Serializable]
public class EffectChainLink
{
    [SerializeReference,SubclassSelector]
    public EffectScript effect;

    [SerializeReference] //Necesario para que serialice bien
    public EffectChainLink next;

    
}