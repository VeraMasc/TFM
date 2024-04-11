using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CustomInspector;
using Effect;
using UnityEngine;

/// <summary>
/// Identifica una cadena de efectos
/// </summary>
[Serializable]
public class EffectChain
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

    public EffectChain(){
        list = new List<EffectScript>();
    }

    public EffectChain( IEnumerable<EffectScript> effects){
        list=effects.ToList();
    }

    /// <summary>
    /// Devuelve un clon de la cadena actual
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public EffectChain clone(){
        var cloneList = list.Select( effect => effect.clone());
        return new EffectChain(cloneList);
        
    }
    
    /// <summary>
    /// Obtiene todas las partes de la cadena de efectos que requieren inputs del jugador
    /// </summary>
    /// <returns> lista de los inputs recuperados</returns>
    public List<Effect.IManual> getManualInputs(){
        return list.SelectMany(effect => effect.getManualInputs()).ToList();
    }

    /// <summary>
    /// Copia la secuencia de efectos como una cadena independiente
    /// </summary>
    public EffectChain cloneFrom(IEnumerable<EffectScript> source){
        throw new NotImplementedException();
        //TODO: implementar clonación de efectos
    }
    
    /// <summary>
    /// Precalcula las partes precalculables del código
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public IEnumerator precalculate(Context context){
        yield return null;
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