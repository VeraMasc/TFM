using System.Collections;
using System.Collections.Generic;
using CardHouse;
using UnityEngine;

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
    [SerializeReference, SubclassSelector]
    public CardCastEffect usageEffect ;
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