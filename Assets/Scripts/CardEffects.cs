using System.Collections;
using System.Collections.Generic;
using CardHouse;
using CustomInspector;
using UnityEngine;
using System.Linq;
using System;

/// <summary>
/// Describe los posibles efectos de una carta
/// </summary>
[RequireComponent(typeof(Card))]
public class CardEffects : MonoBehaviour
{
    
    /// <summary>
    /// Efecto básico que producir al usar la carta 
    /// //TODO: limitar a CardCast y Modales
    /// </summary>
    [Button(nameof(displayValues))]
    [SerializeReference, SubclassSelector]
    public CardCastEffect usageEffect ;

    /// <summary>
    /// Contiene los distintos valores que se puede escoger
    /// </summary>
   
    public Dictionary<Descriptor.ValueName,Descriptor.IStoredValue> values = new Dictionary<Descriptor.ValueName, Descriptor.IStoredValue>();

    public List<string> displayValues(){
        var valueList= values.Select(kvp => $"{Enum.GetName(typeof(Descriptor.ValueName),kvp.Key)}: {kvp.Value.stringValue()}");
        Debug.Log(String.Join("\n",valueList.ToList() ));
        return valueList.ToList();
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