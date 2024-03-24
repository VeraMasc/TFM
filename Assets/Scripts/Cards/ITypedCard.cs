using System.Collections;
using System.Collections.Generic;
using CardHouse;
using UnityEngine;

/// <summary>
/// Identifica a las cartas con tipo
/// </summary>
public interface ITypedCard
{
    /// <summary>
    /// Nombre de la carta
    /// </summary>
    public string cardName {get;set;}
    /// <summary>
    /// Obtiene el tipo de la carta
    /// </summary>
    public HashSet<string> cardType {get;set;}
}
